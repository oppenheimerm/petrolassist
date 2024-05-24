using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.Datastore.EFCore.Repositories;
using PA.UseCases.Interfaces;
using PA.UseCases.PetrolStationUseCase;

namespace PA.Web.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization.Attributes.Authorize]
    public class StationsController : ControllerBase
    {
        readonly IGetNearestStationsUseCase GetNearestStationsUsecase;
        readonly IGetStationByIdExternalUseCase GetStationByExternalId;

		public StationsController(IGetNearestStationsUseCase getNearestStationsUseCase, IGetStationByIdExternalUseCase getStationByIdExternalUseCase)
		{
			GetNearestStationsUsecase = getNearestStationsUseCase;
			GetStationByExternalId = getStationByIdExternalUseCase;
		}


		//[HttpPost("refresh-token")]
		[AllowAnonymous]
        [HttpGet("get-nearest-stations")]
        public List<StationLite> GetNearestStations(double fromLat, double fromLongt,
		DistanceUnit units)
		{
            return GetNearestStationsUsecase.Execute(fromLat, fromLongt, units);
        }

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<StationLite>> Station(Guid? id)
		{
			if (!id.HasValue)
			{
				return NotFound();
			}
			else
			{

				var station =  await GetStationByExternalId.ExecuteAsync(id.Value);
				if(station == null)
				{
					return NotFound();
				}
				else
				{
					return station;
				}
			}
			
		}

	}
}
