using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Pages.Vendors
{
    public class IndexModel : PageModel
    {
        private readonly IGetAllVendorsPagingUseCase GetAllVendorsPagingUseCase;
		readonly IConfiguration Configuration;


		int? Spread { get; set; }
		public List<PagingLink>? Links { get; set; }
        public VendorSortOrder SortOrder { get; set; }
		public PagingParameters? IndexPagingParameters { get; set; }
		public PagingResponse<Vendor>? PagedVendorsList { get; set; }
		public MetaData? VendorsMetaData { get; set; }
		public int? Id { get; set; }

		public IndexModel(IGetAllVendorsPagingUseCase getAllVendorsPagingUseCase, IConfiguration configuration)
        {
            GetAllVendorsPagingUseCase = getAllVendorsPagingUseCase;
            Configuration = configuration;
            Spread = 2;
        }

		public void OnGet(int? id, int? sortOrder, int? pageNumber)
		{
			SortOrder = (!sortOrder.HasValue) ? VendorSortOrder.Id : PaginHelpers.GetVendorSortOrder(sortOrder.Value);

			IndexPagingParameters = new()
			{
				PageSize = int.Parse(Configuration["ApplicationSettings:PageSize"]!),
				PageNumber = (pageNumber.HasValue) ? pageNumber.Value : 1
			};

			// Get Enum sort type

			//	To help with the function call below, in the initial load case, where sortOrder is null
			int initSortOrder = (!sortOrder.HasValue) ? 0 : sortOrder.Value;
			var vendors = GetAllVendorsPagingUseCase.Execute(id, initSortOrder, IndexPagingParameters);

			var pagingResponse = new PagingResponse<Vendor>
			{
				Items = vendors,
				MetaData = vendors.MetaData
			};

			VendorsMetaData = vendors.MetaData;
			CreatePaginationLinks();
			PagedVendorsList = pagingResponse;
		}


		/// <summary>
		/// Create a new _links variable that will hold all the links for our pagination component. As soon as
		//  our parameters get their values, the OnParameterSet lifecycle method will run and call the
		//  CreatePaginationLinks method. In that method, we create the Previous link, the page number links with
		//  the Active property set to true for the current page and the Next link. Additionally, we have the
		//  OnSelectedPage method.
		/// </summary>
		/// <param name="link"></param>
		/// <returns></returns>
		void CreatePaginationLinks()
		{
			Links = new List<PagingLink>();
			Links.Add(new PagingLink(VendorsMetaData.CurrentPage - 1, VendorsMetaData.HasPrevious, "<"));
			for (int i = 1; i <= VendorsMetaData.TotalPages; i++)
			{
				if (i >= VendorsMetaData.CurrentPage - Spread && i <= VendorsMetaData.CurrentPage + Spread)
				{
					Links.Add(new PagingLink(i, true, i.ToString()) { Active = VendorsMetaData.CurrentPage == i });
				}
			}
			Links.Add(new PagingLink(VendorsMetaData.CurrentPage + 1, VendorsMetaData.HasNext, ">"));
		}
	}
}
