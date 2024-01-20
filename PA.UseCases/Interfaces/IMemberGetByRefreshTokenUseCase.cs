using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IMemberGetByRefreshTokenUseCase
    {
        Task<AuthenticateResponse> ExecuteAsync(string refreshToken, string ipAddress);
    }
}
