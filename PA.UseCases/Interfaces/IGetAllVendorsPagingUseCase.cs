
using PA.Core.Helpers.Paging;
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IGetAllVendorsPagingUseCase
	{
		PagedList<Vendor> Execute(int? Id, int? sortingOrder, PagingParameters pagingParameters);
	}
}
