
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorsUseCase
{
	public class AddVendorUseCase : IAddVendorUseCase
	{
		private readonly IVendorRepository VendorRepository;

        public AddVendorUseCase(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }

		public async Task<(Vendor Vendor, bool Success, string ErrorMessage)> ExecuteAsync(Vendor vendor)
		{
			var response = await VendorRepository.Add(vendor);
			return response;
		}
	}
}
