using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PA.Core.Helpers;
using PA.Core.Interfaces.Services.Email;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;
using System.Net;
using System.Text.Json;


namespace PA.Web.API.Controllers
{
    //  Base account controller: https://github.com/cornflourblue/aspnet-core-3-signup-verification-api/blob/master/Controllers/BaseController.cs
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMemberRegisterUseCase MemberRegisterUC;
        private readonly IMemberAuthenticateUserCase MemberAuthenticateUC;
        private readonly IMemberGetByRefreshTokenUseCase MemberRefreshTokenUC;
        private readonly IMemberVerifyEmailUseCase MemberVerifyEmailUC;
        private readonly IIsMemberEmailVerfiedUseCase MemberEmailVerifiedUC;
        private readonly IEmailService EmailService;

		public AccountController(IMemberRegisterUseCase memberRegisterUseCase, IMemberAuthenticateUserCase memberAuthenticateUserCase,
            IMemberGetByRefreshTokenUseCase memberRefreshTokenUC, IMemberVerifyEmailUseCase memberVerifyEmailUseCase,
			IIsMemberEmailVerfiedUseCase emailVerfiedUseCase, IEmailService emailService)
        {
            MemberRegisterUC = memberRegisterUseCase;
            MemberAuthenticateUC = memberAuthenticateUserCase;
            MemberRefreshTokenUC = memberRefreshTokenUC;
            MemberVerifyEmailUC = memberVerifyEmailUseCase;
            MemberEmailVerifiedUC = emailVerfiedUseCase;
            EmailService = emailService;
        }



		/*[AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {

            var (Errors, FormValid) = RegisterRequestValidation.IsFormValid(model);

			if (FormValid == true)
            {
				var response = await MemberRegisterUC.ExecuteAsync(model, ipAddress());
				if (response.Success)
				{
					return Ok(new { message = TextStrings.RegistrationSuccessful_EN });

				}
				else
				{
					return BadRequest(new { message = response.ErrorMessage });
				}
            }
            else
            {
				return BadRequest(new { message = Errors.ToJson() });
			}

        }*/


		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromForm] RegisterRequest model)
		{
            var formValid = RegisterRequestValidation.IsFormValid(model);
            if(formValid.FormValid == true)
            {
				var response = await MemberRegisterUC.ExecuteAsync(model, ipAddress());
				if (response.Success)
				{
					return Ok(new { message = TextStrings.RegistrationSuccessful_EN });

				}
				else
				{
                    // Email in user error / Mobile in user error
					return BadRequest(new { message = response.ErrorMessage});
				}
			}
            else
            {
				return BadRequest(new { message = JsonSerializer.Serialize(formValid.Errors) });
			}

		}

		[AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {

            var response = await MemberAuthenticateUC.ExecuteAsync(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            if (response.StatusCode == 200)//OK
            {
				return Ok(response);
            }
            else if(response.StatusCode == 401  && response.Message == TextStrings.EmailNotVerified)
			{
                return Unauthorized(TextStrings.EmailNotVerified);
            }
            else
            {
                return NotFound(response);//404
            }
            
        }


        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await  MemberRefreshTokenUC.ExecuteAsync(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            if (response.StatusCode == 401)
            {
                return Unauthorized();
            }
            else {
                return Ok(response);
            }
        }

		[AllowAnonymous]
		[HttpPost("verify-email")]
		public async Task<IActionResult> VerifyEmail(string email)
		{
            var request = await MemberVerifyEmailUC.ExecuteAsync(email);
            if (request.Success)
            {
                return Ok(new { message = request.Message });
            }
            else
            {
                if (request.httpCode == 400)
                {
                    return BadRequest(new { message = request.Message });
                }
                else if(request.httpCode == 404)
                {
					return NotFound(new { message = request.Message });
                }
                else
                {
					return NotFound(new { message = request.Message });
				}
            }
		}

        [AllowAnonymous]
        [HttpGet("resend-email")]
        public async Task<IActionResult>ResendEmail(string email)
        {
            //  Test
            //  non account - OK
            //  correct account - OK

            // TODO
            //  Use queue for email sending

            var response = await MemberEmailVerifiedUC.ExecuteAsync(email);
            if(response.hasAccount == true)
            {
                // resend OK for now but need to add to a queueService
                // only resend if not verified
                if(response.Verified == true)
                {
                    // TO TEST
					return Ok(new { message = TextStrings.EmaiVerified });
                }
                else
                {
                    // TO TEST
                    //Resend
                    resendEmailVerification(response.Member, Request.Headers["origin"]);
					return Ok(new { message = TextStrings.EmailVerificationTokenResent });
				}
                
            }
            else
            {
                return NotFound( new { message = TextStrings.UserNotFound } );
            }

		}


		#region Helpers
		private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Helper method appends an HTTP Only cookie containing the refresh token to the response for 
        /// increased security. HTTP Only cookies are not accessible to client-side javascript which 
        /// prevents XSS (cross site scripting), and the refresh token can only be used to fetch a 
        /// new token from the <see cref="UsersController.RefreshToken"/> route which prevents 
        /// CSRF (cross site request forgery).
        /// </summary>
        /// <param name="token"></param>
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }


        private void resendEmailVerification(Member member, string? origin)
        {
			string message;
            if (!string.IsNullOrEmpty(origin))
            {

                var verifyUrl = $"{origin}/account/verify-email?token={member.VerificationToken}";
				message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
			}
			else
			{
				message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{member.VerificationToken}</code></p>";
			}

            EmailService.Send(
				to: member.EmailAddress,
				subject: "Sign-up Verification API - Verify Email",
				html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
				);

		}
        #endregion
    }
}
