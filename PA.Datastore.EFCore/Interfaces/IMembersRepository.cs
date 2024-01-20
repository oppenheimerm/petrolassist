
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
        Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterRequest model, string origin);
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
        IQueryable<Member> GetAll();
        Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName);
    }
}
