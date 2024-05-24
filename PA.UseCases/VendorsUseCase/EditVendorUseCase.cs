
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorsUseCase
{
	public class EditVendorUseCase : IEditVendorUseCase
	{
		private readonly IVendorRepository VendorRepository;


		public EditVendorUseCase(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }

		public async Task<(Vendor Vendor, bool success, string ErrorMessage)> ExecuteAsync(Vendor vendor)
		{
			var response = await VendorRepository.Edit(vendor);
			return response;
		}
	}
}
