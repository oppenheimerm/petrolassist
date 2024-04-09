
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IMemberRegisterUseCase
    {
		Task<(bool Success, string ErrorMessage, int ErrorCode)> ExecuteAsync(RegisterRequest request, string origin);
    }
}
