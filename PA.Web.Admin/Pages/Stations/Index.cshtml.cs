using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Pages.Stations
{
	public class IndexModel : PageModel
	{
		readonly IConfiguration Configuration;


		private readonly IGetAllStationsUseCase GetAllStationsUseCase;

		public PagingResponse<Station>? PagedStationList { get; set; }
		public List<PagingLink>? Links { get; set; }
		public MetaData? StationMetaData { get; set; }
		int? Spread { get; set; }
		public int? CountryCode { get; set; } // Leave null for all for now
											  //var sortOrder = sortingOrder.HasValue ? PaginHelpers.GetStationSortOrder(sortingOrder.Value) : StationSortOrder.Id;
		public StationSortOrder SortOrder { get; set; }
		public PagingParameters? IndexPagingParameters { get; set; }

		public IndexModel(IGetAllStationsUseCase getAllStationsUseCase, IConfiguration configuration)
		{
			GetAllStationsUseCase = getAllStationsUseCase;
			Configuration = configuration;
			Spread = 2;
		}

		public void OnGet(int? countryCode, int? sortOrder, int? pageNumber)
		{
			SortOrder = (!sortOrder.HasValue) ? StationSortOrder.Id : PaginHelpers.GetStationSortOrder(sortOrder.Value);

			IndexPagingParameters = new()
			{
				PageSize = int.Parse(Configuration["ApplicationSettings:PageSize"]!),
				PageNumber = (pageNumber.HasValue) ? pageNumber.Value : 1
			};

			// Get Enum sort type

			//	To help with the function call below, in the initial load case, where sortOrder is null
			int initSortOrder = (!sortOrder.HasValue) ? 0 : sortOrder.Value;
			var stations = GetAllStationsUseCase.Execute(countryCode, initSortOrder, IndexPagingParameters);

			var pagingResponse = new PagingResponse<Station>
			{
				Items = stations,
				MetaData = stations.MetaData
			};

			StationMetaData = stations.MetaData;
			CreatePaginationLinks();

			PagedStationList = pagingResponse;
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
			Links.Add(new PagingLink(StationMetaData.CurrentPage - 1, StationMetaData.HasPrevious, "<"));
			for (int i = 1; i <= StationMetaData.TotalPages; i++)
			{
				if (i >= StationMetaData.CurrentPage - Spread && i <= StationMetaData.CurrentPage + Spread)
				{
					Links.Add(new PagingLink(i, true, i.ToString()) { Active = StationMetaData.CurrentPage == i });
				}
			}
			Links.Add(new PagingLink(StationMetaData.CurrentPage + 1, StationMetaData.HasNext, ">"));
		}
	}
}
