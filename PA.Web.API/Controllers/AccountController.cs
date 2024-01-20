using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;
using System.Net;


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

        public AccountController(IMemberRegisterUseCase memberRegisterUseCase, IMemberAuthenticateUserCase memberAuthenticateUserCase,
            IMemberGetByRefreshTokenUseCase memberRefreshTokenUC)
        {
            MemberRegisterUC = memberRegisterUseCase;
            MemberAuthenticateUC = memberAuthenticateUserCase;
            MemberRefreshTokenUC = memberRefreshTokenUC;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var response = await  MemberRegisterUC.ExecuteAsync(model, ipAddress());
            if(response.Success)
            {
                return Ok(new { message = "Registration successful, please check your email for verification instructions" });

            }
            else
            {
                return BadRequest(new { message = response.ErrorMessage });
            }
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await MemberAuthenticateUC.ExecuteAsync(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);            
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

        #endregion
    }
}
