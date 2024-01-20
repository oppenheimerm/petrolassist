
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IMemberAuthenticateUserCase
    {
        Task<AuthenticateResponse> ExecuteAsync(AuthenticateRequest model, string ipAddress);
    }
}
