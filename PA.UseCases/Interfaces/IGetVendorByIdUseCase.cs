
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IGetVendorByIdUseCase
	{
		Task<Vendor?> ExecuteAsync(int Id);
	}
}
