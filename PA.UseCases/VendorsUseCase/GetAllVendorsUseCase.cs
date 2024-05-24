
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorsUseCase
{
    public class GetAllVendorsUseCase : IGetAllVendorsUseCase
    {
        private readonly IVendorRepository VendorRepository;
        public GetAllVendorsUseCase(IVendorRepository vendorRepository)
        {
            VendorRepository = vendorRepository;
        }


		public IQueryable<Vendor> Execute()
		{
			return VendorRepository.GetAll();
		}
	}
}
