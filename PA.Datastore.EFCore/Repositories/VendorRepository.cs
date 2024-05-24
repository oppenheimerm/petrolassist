

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Datastore.EFCore.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ILogger<VendorRepository> Logger;
        private readonly ApplicationManagmentDbContext Context;

        public VendorRepository(ILogger<VendorRepository> logger, ApplicationManagmentDbContext contenxt)
        {
            Logger = logger;
            Context = contenxt;
        }

		public IQueryable<Vendor> GetAll()
		{
			var vendors = Context.PetrolVendors
				.Include(c => c.Country)
				.AsNoTracking();
			return vendors; ;
		}

		public async Task<Vendor?> GetVendorByIdAsync(int id)
		{
			return await Context.PetrolVendors.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
		}

		public PagedList<Vendor> GetAll(int?Id, int? sortingOrder, PagingParameters pagingParameters)
        {

			// Get Enum sort type
			var sortOrder = sortingOrder.HasValue ? PaginHelpers.GetVendorSortOrder(sortingOrder.Value) : VendorSortOrder.Id;

            if(Id == null)
            {
				var vendors = Context.PetrolVendors
					.Include(c => c.Country)
					.AsNoTracking();

				// nullable parameter, we want all.
				switch (sortOrder)
                {
                    case VendorSortOrder.Id:
                        vendors = vendors.OrderBy(v => v.Id);
                        break;
                    case VendorSortOrder.VendorName:
                        vendors = vendors.OrderBy(v => v.VendorName);
                        break;
                    case VendorSortOrder.VendorCode:
                        vendors = vendors.OrderBy(v => v.VendorCode);
                        break;
                    default:
                        vendors = vendors.OrderBy(v => v.Id);
                        break;
				}

                return PagedList<Vendor>.ToPagedList(vendors, pagingParameters.PageNumber, pagingParameters.PageSize);
            }
            else
            {
				var vendors = Context.PetrolVendors
					.Include(c => c.Country)
					.AsNoTracking();

				switch (sortOrder)
				{
					case VendorSortOrder.Id:
						vendors = vendors.OrderBy(v => v.Id);
						break;
					case VendorSortOrder.VendorName:
						vendors = vendors.OrderBy(v => v.VendorName);
						break;
					case VendorSortOrder.VendorCode:
						vendors = vendors.OrderBy(v => v.VendorCode);
						break;
					default:
						vendors = vendors.OrderBy(v => v.Id);
						break;
				}

				return PagedList<Vendor>.ToPagedList(vendors, pagingParameters.PageNumber, pagingParameters.PageSize);
			}
        }

        public async Task<(Vendor vendor, bool Success, string ErrorMessage)> Add(Vendor vendor)
        {
            // unique vendor code?
            var isUnique = await IsVendorCodeUnique(vendor.VendorCode);
            if (isUnique)
            {
                try
                {
                    Context.PetrolVendors.Add(vendor);
                    await Context.SaveChangesAsync();
                    Logger.LogInformation($"Vendor with Id: {vendor.Id}, and vendor code of: {vendor.VendorCode}added to database at: {DateTime.UtcNow}");
                    return (vendor, true, string.Empty);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to add vendor to database. Timestamp : {DateTime.UtcNow}");
                    return (vendor, false, ex.ToString());
                }
            }
            else
            {
                var errMsg = $"Failed to add vendor with vendor code {vendor.VendorCode} to database.  Code already in use. Timestamp : {DateTime.UtcNow}";
                Logger.LogError(errMsg);
                return (vendor, false, errMsg);
            }

        }


		public async Task<(Vendor vendor, bool Success, string ErrorMessage)> Edit(Vendor vendor)
		{
			// unique vendor code?
			var isUnique = await IsVendorCodeUnique(vendor.VendorCode);
			if (isUnique)
			{
				try
				{
					Context.PetrolVendors.Update(vendor);
					await Context.SaveChangesAsync();
					Logger.LogInformation($"Vendor with Id: {vendor.Id}, and vendor code of: {vendor.VendorCode} was edited  at: {DateTime.UtcNow}");
					return (vendor, true, string.Empty);
				}
				catch (Exception ex)
				{
					Logger.LogError($"Failed to update vendor. Timestamp : {DateTime.UtcNow}");
					return (vendor, false, ex.ToString());
				}
			}
			else
			{
				var errMsg = $"Vendor codes must be unique. Vendor Code: {vendor.VendorCode} already in use. Timestamp : {DateTime.UtcNow}";
				Logger.LogError(errMsg);
				return (vendor, false, errMsg);
			}

		}



		// Helpers
		public async Task<bool> IsVendorCodeUnique(string vendorCode)
        {
            var codeInUse = await Context.PetrolVendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorCode == vendorCode.ToUpper());

            return (codeInUse == null) ? true : false;
        }
    }
}
