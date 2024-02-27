using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.UseCases.Interfaces;


namespace PA.Web.Admin.Controllers
{
	[Route("api/stations")]
	[ApiController]
	// [Authorize]
	public class StationsController : ControllerBase
	{
		private readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
		private readonly IGetAllStationsUseCase GetAllStationsUseCase;
		private readonly IAddPetrolStationUseCase AddPetroStationUseCase;

		public StationsController(IGetAllCountriesUseCase getAllCountriesUseCase, IGetAllStationsUseCase getAllStationsUseCase,
					IAddPetrolStationUseCase addPetrolStationUseCase)
		{
			GetAllCountriesUseCase = getAllCountriesUseCase;
			GetAllStationsUseCase = getAllStationsUseCase;
			AddPetroStationUseCase = addPetrolStationUseCase;
		}


		[HttpGet("all")]
		public IActionResult GetStations(int? countryCode, int? sortingOrder, int pageNumber, int pageSize)
		{
			// TODO Catch Null
			//  pageNumber
			//  pageSize

			PagingParameters pagingParameters = new()
			{
				PageSize = pageSize,
				PageNumber = pageNumber
			};

			// Get Enum sort type
			var sortOrder = sortingOrder.HasValue ? PaginHelpers.GetStationSortOrder(sortingOrder.Value) : StationSortOrder.Id;
			var stations = GetAllStationsUseCase.Execute(countryCode, sortingOrder, pagingParameters);


			var pagingResponse = new PagingResponse<Station>
			{
				Items = stations,
				MetaData = stations.MetaData
			};

			//https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
			return Ok(pagingResponse);
		}

		[HttpGet("countries")]
		public IQueryable<Country> GetCountries()
		{
			return GetAllCountriesUseCase.Execute();
		}

		/*[HttpPost("add-petrol-station")]
        public async IActionResult<AddStationReplyRequest> AddStation(AddPetrolStationRequest addPetrolStationRequest) {


            return await AddPetroStationUseCase.ExecuteAsync()

		}*/
	}
}
