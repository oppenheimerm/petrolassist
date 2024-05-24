using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.VendorsUseCase
{
	public class GetAllVendorsPagingUseCase : IGetAllVendorsPagingUseCase
	{
		private readonly IVendorRepository VendorRepository;

		public GetAllVendorsPagingUseCase(IVendorRepository vendorRepository)
        {
			VendorRepository = vendorRepository;
		}

		public PagedList<Vendor> Execute(int? Id, int? sortingOrder, PagingParameters pagingParameters)
		{
			return VendorRepository.GetAll(Id, sortingOrder, pagingParameters);
		}
	}
}

