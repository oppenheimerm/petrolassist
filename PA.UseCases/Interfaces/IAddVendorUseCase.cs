
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IAddVendorUseCase
	{
		Task<(Vendor Vendor, bool Success, string ErrorMessage)> ExecuteAsync(Vendor vendor);
	}
}
