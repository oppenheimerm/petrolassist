
using PA.Core.Helpers.Paging;
using PA.Core.Models;

namespace PA.Datastore.EFCore.Interfaces
{
    public interface IVendorRepository
    {
        Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor);
        Task<(Vendor vendor, bool Success, string ErrorMessage)> Edit(Vendor vendor);

		Task<bool> IsVendorCodeUnique(string vendorCode);
        Task<Vendor?> GetVendorByIdAsync(int id);

		PagedList<Vendor> GetAll(int? Id, int? sortingOrder, PagingParameters pagingParameters);
        IQueryable<Vendor> GetAll();

	}
}
