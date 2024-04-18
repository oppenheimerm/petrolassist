using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PA.Core.Helpers;
using PA.Core.Models.ApiRequestResponse;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UtilityLibary;
using System.Net;
using PA.Datastore.EFCore.Authorisation.Interfaces;

namespace PA.Datastore.EFCore.Repositories
{
	public class MembersRepository : IMembersRepository
	{
		private readonly ILogger<MembersRepository> Logger;
		private readonly ApplicationManagmentDbContext Context;
		private readonly IJwtUtils JwtUtilis;
		private readonly IConfiguration Configuration;
		private IWebHostEnvironment Environment;
		private readonly IPhotoFileRepository PhotoFileRepository;

        public MembersRepository(ILogger<MembersRepository> logger, ApplicationManagmentDbContext context,
			IJwtUtils jwtUtils, IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
			IPhotoFileRepository photoFileRepository)
        {
			Logger = logger;
			Context = context;
			JwtUtilis = jwtUtils;
			Configuration = configuration;
			Environment = webHostEnvironment;
			PhotoFileRepository = photoFileRepository;
		}

		public async Task<(bool Success, string ErrorMessage, int ErrorCode)> RegisterAsync(RegisterRequest model, string origin)
		{


			// validate
			if (Context.Members.Any(x => x.EmailAddress == model.EmailAddress.ToLowerInvariant()))
			{
				return (false, TextStrings.EmailAlreadyInUse, 400);
			}

			if (Context.Members.Any(x => x.MobileNumber == model.MobileNumber))
			{
				return (false, TextStrings.MobilePhoneAlreadyInUse, 400);
			}

			//  Must accept terms
			if (model.AcceptTerms == false)
			{
				return (false, TextStrings.MustAcceptTOS, 400);
			}

			// map model to new account object
			var account = model.ToMember();


			account.Created = DateTime.Now;
			var randomString = new RandomStringGenerator();
			account.VerificationToken = randomString.Generate(13);

			account.MemberPhoto = string.Empty;



			//  TODO https://www.tutorialspoint.com/Chash-program-to-check-the-validity-of-a-Password

			// hash password
			account.PasswordHash = BC.HashPassword(model.Password);

			// save account
			await Context.Members.AddAsync(account);
			await Context.SaveChangesAsync();
			Logger.LogInformation($"Sucessfully added new user: {model.EmailAddress} at: {DateTime.UtcNow}");


			//  TODO
			// send email
			//sendVerificationEmail(account, origin);

			return (true, string.Empty, 200);
		}

		public async Task<AuthenticateResponse> LogInAsync(AuthenticateRequest model, string ipAddress)
		{
			var errorMessage = string.Empty;
			var rsp = new AuthenticateResponse();
			var user = await Context.Members.SingleOrDefaultAsync(m => m.EmailAddress == model.EmailAddress.ToLowerInvariant());
			if (user == null)
			{
				errorMessage = TextStrings.UserNotFound ; 
				Logger.LogError($"Failed login attempt failed for Email: {model.EmailAddress} at the following IPAddress: {ipAddress}. Timestamp : {DateTime.UtcNow}");
				rsp.StatusCode = (int)HttpStatusCode.NotFound;
				rsp.Message = errorMessage;
				rsp.Success = false;
				return rsp;
			}

			//  Verify that user has verified their email address
			if (user.Verified == null)
			{
				//  Account not verified
				errorMessage = TextStrings.EmailNotVerified;
				Logger.LogError($" User {user.EmailAddress} attempted to login without email verfication. Timestamp : {DateTime.UtcNow}");
				rsp.StatusCode = (int)HttpStatusCode.Unauthorized;
				rsp.Message = errorMessage;
				rsp.Success = false;
				return rsp;
			}

			// validate
			if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
			{
				Logger.LogError($"Failed login attempt failed for Email: {model.EmailAddress}. Timestamp : {DateTime.UtcNow}");
				//throw new AppException("Email or password is incorrect");                
				errorMessage = "Email or password is incorrect";
				rsp.StatusCode = (int)HttpStatusCode.Unauthorized;
				rsp.Message = errorMessage;
				rsp.Success = false;
				return rsp;
			};


			// authentication successful so generate jwt and refresh tokens
			var jwtToken = JwtUtilis.GenerateJwtToken(user);
			var refreshToken = await JwtUtilis.GenerateRefreshTokenAsync(ipAddress);
			user?.RefreshTokens?.Add(refreshToken);
			user.LastLogIn = DateTime.UtcNow;

			// remove old refresh tokens from user
			removeOldRefreshTokens(user);

			Context.Update(user);
			await Context.SaveChangesAsync();
			Logger.LogInformation($"User: {model.EmailAddress}, login at {DateTime.UtcNow}");


			//return new AuthenticateResponse(user, jwtToken, refreshToken.Token, errorMessage, (int)HttpStatusCode.OK);


			var userResponse = GetAuthenticatedResponse(
				user,
				jwtToken,
				refreshToken.Token,
				(int)HttpStatusCode.OK,
				true
				);

			return userResponse;

		}

		public async Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress)
		{
			if (string.IsNullOrEmpty(token))
			{
				var responeError = new AuthenticateResponse()
				{
					Message = "valid toke not found",
					StatusCode = 401,
					Success = false
				};
				return responeError;
			}
			var response = await GetUserByRefreshTokenAsync(token);
			if (response.Success)
			{
				var refreshToken = response.user?.RefreshTokens?.Single(x => x.Token == token);

				if (refreshToken.IsRevoked)
				{
					// revoke all descendant tokens in case this token has been compromised
					revokeDescendantRefreshTokens(refreshToken, response.user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
					Context.Update(response.user);
					await Context.SaveChangesAsync();
				}

				if (!refreshToken.IsActive)
					Logger.LogError($"Invalid Toke Exception raised at: {DateTime.UtcNow}");

				// replace old refresh token with a new one (rotate token)
				var newRefreshToken = await RotateRefreshTokenAsync(refreshToken, ipAddress);
				response.user.RefreshTokens.Add(newRefreshToken);

				// remove old refresh tokens from user
				removeOldRefreshTokens(response.user);

				// save changes to db
				Context.Update(response.user);
				await Context.SaveChangesAsync();

				// generate new jwt
				var jwtToken = JwtUtilis.GenerateJwtToken(response.user);
				var authResponse = GetAuthenticatedResponse(response.user, jwtToken, newRefreshToken.Token, (int)HttpStatusCode.OK);
				authResponse.Success = true;

				return authResponse;
			}
			else
			{
				var responeError = new AuthenticateResponse()
				{
					Message = response.ErrorMessage,
					StatusCode = 404
				};
				return responeError;
			}
		}


		public async Task<(bool Success, string Message, int httpCode)> VerifyAccountToken(string token)
		{
			if (token == null)
			{
				var error = $"Failed validation, no token found. TimeStamp: {DateTime.UtcNow}";
				Logger.LogError(error);
				return (false, TextStrings.BadRequest, 400);
			}

			var account = await Context.Members.SingleOrDefaultAsync(x => x.VerificationToken == token);

			if (account == null)
			{
				var error = $"Account not found for token. TimeStamp: {DateTime.UtcNow}";
				Logger.LogError(error);
				return (false, TextStrings.EmailNotVerificationTokenNotFound, 404);
			}

			account.Verified = DateTime.UtcNow;
			account.VerificationToken = string.Empty;

			Context.Members.Update(account);
			await Context.SaveChangesAsync();
			var msg = $"Succesfuly validated account. TimeStamp: {DateTime.UtcNow}";
			return (true, msg, 200);
		}



		public IQueryable<Member> GetAll()
		{
			return Context.Members.AsNoTracking().OrderBy(m => m.LasttName);
		}


		/// <summary>
		/// Used by <see cref=" PA.Datastore.EFCore.Authorisation.Interfaces.JwtMiddleware"/>
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<Member?> GetMemberByIdAsync(Guid Id)
		{
			return await Context.Members
				.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Id);
		}

		public async Task<Member?> GetMemberByEmail(string email)
		{
			return  await Context.Members
				.AsNoTracking().FirstOrDefaultAsync(e => e.EmailAddress.ToLower() == email.ToLower());
		}


		public async Task<(bool Verified, bool hasAccount, Member? member)> MemberEmailVerified(string email)
		{
			var member = await Context.Members
				.AsNoTracking().FirstOrDefaultAsync(e => e.EmailAddress.ToLower() == email.ToLower());
			if(member != null)
			{
				//var emailVerified = false;
				var verified = (member.IsVerified == true) ? true : false;
				return(verified, true, member);
			}
			else
			{
				return(false, false, null);
			}

		}

		public async Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName)
		{
			if (member != null)
			{
				member.MemberPhoto = photoName;
				member.Updated = DateTime.Now;
				Context.Members.Update(member);
				await Context.SaveChangesAsync();
				Logger.LogInformation($"Member with Id: {member.Id}, updated photo at: {DateTime.UtcNow}");
				return (member, true, string.Empty);
			}
			else
			{
				Logger.LogError($"Failed to locate update member photo.  The passed in member parameter is null. Timestamp: {DateTime.UtcNow}");
				return (null, false, "Could not find user");
			}
		}

		#region helpers

		private async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken, string ipAddress)
		{
			var newRefreshToken = await JwtUtilis.GenerateRefreshTokenAsync(ipAddress);
			revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
			return newRefreshToken;
		}

		private void sendVerificationEmail(Member account, string origin)
		{
			string message;
			if (!string.IsNullOrEmpty(origin))
			{
				var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
				message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
			}
			else
			{
				message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
			}

			/*_emailService.Send(
                to: account.EmailAddress,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );*/
		}

		private void removeOldRefreshTokens(Member user)
		{
			// remove old inactive refresh tokens from user based on TTL in app settings
			user.RefreshTokens.RemoveAll(x =>
				!x.IsActive &&
				x.Created.AddDays(int.Parse(Configuration["WebApiSecurity:RefreshTokenTTL"])) <= DateTime.UtcNow);
		}

		private AuthenticateResponse GetAuthenticatedResponse(Member user, string jwtToken, string newRefreshToken, int statusCode, bool success = false)
		{
			return new AuthenticateResponse()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LasttName,
				JwtToken = jwtToken,
				Photo = user.MemberPhoto,
				EmailAddress = user.EmailAddress,
				RefreshToken = newRefreshToken,
				Message = string.Empty,
				StatusCode = statusCode,
				MobileNumber = user.MobileNumber,
				DistanceUnit = user.DistanceUnit,
				Success = success
			};
		}


		private async Task<(Member user, bool Success, string ErrorMessage)> GetUserByRefreshTokenAsync(string token)
		{
			var user = await Context.Members.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				var errMsg = $"Token inavlid {token}!.  TimeStamp: {DateTime.UtcNow}";
				Logger.LogError(errMsg);
				return (new Member(), false, errMsg);
			}

			return (user, true, string.Empty);
		}

		private void revokeDescendantRefreshTokens(RefreshToken refreshToken, Member user, string ipAddress, string reason)
		{
			// recursively traverse the refresh token chain and ensure all descendants are revoked
			if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
			{
				var childToken = user.RefreshTokens?.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
				if (childToken.IsActive)
					revokeRefreshToken(childToken, ipAddress, reason);
				else
					revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
			}
		}

		private void revokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
		{
			token.Revoked = DateTime.UtcNow;
			token.RevokedByIp = ipAddress;
			token.ReasonRevoked = reason;
			token.ReplacedByToken = replacedByToken;
			Logger.LogInformation($"Token {token.Token} revoked at {DateTime.UtcNow}. Reason: {token.ReasonRevoked}");
		}

		#endregion
	}
}
