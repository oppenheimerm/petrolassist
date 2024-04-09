
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;

namespace PA.Datastore.EFCore.Interfaces
{
    public interface IMembersRepository
    {
		/// <summary>
		/// <see cref="Member"/> Registration.  Returns true o
		/// </summary>
		/// <param name="model"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		Task<(bool Success, string ErrorMessage, int ErrorCode)> RegisterAsync(RegisterRequest model, string origin);
        /// <summary>
        /// Authenticate a <see cref="Member"/>. Returns a <see cref="AuthenticateResponse"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> LogInAsync(AuthenticateRequest model, string ipAddress);
        /// <summary>
        /// Refresh tokens for <see cref="Member"/>.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        /// <summary>
        /// Used by <see cref="PA.Web.API.Authorization.MiddleWare.JwtMiddleware"/>
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Member?> GetMemberByIdAsync(Guid Id);
        /// <summary>
        /// Get user by eamil
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Member?> GetMemberByEmail(string email);

		IQueryable<Member> GetAll();
        Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName);
        /// <summary>
        /// Account token verification token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<(bool Success, string Message, int httpCode)> VerifyAccountToken(string token);

		/// <summary>
		/// Checks if <see cref="Member"/> has verified their email and has a valid account
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		Task<(bool Verified, bool hasAccount, Member? member)> MemberEmailVerified(string email);

	}
}
