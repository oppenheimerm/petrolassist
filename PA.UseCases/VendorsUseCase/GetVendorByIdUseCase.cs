
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorsUseCase
{
	public class GetVendorByIdUseCase : IGetVendorByIdUseCase
	{
		readonly IVendorRepository VendorRepository;

		public GetVendorByIdUseCase(IVendorRepository vendorRepository)
		{
			VendorRepository = vendorRepository;
		}

		public async Task<Vendor?> ExecuteAsync(int Id)
		{
			return await VendorRepository.GetVendorByIdAsync(Id);
		}
	}
}
