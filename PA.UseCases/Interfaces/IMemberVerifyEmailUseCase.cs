
namespace PA.UseCases.Interfaces
{
	public interface IMemberVerifyEmailUseCase
	{
		Task<(bool Success, string Message, int httpCode)> ExecuteAsync(string token);
	}
}
