
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IIsMemberEmailVerfiedUseCase
	{
		Task<(bool Verified, bool hasAccount, Member? Member)> ExecuteAsync(string EmailAddress);
	}
}
