using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.Web.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization.Attributes.Authorize]
    public class StationsController : ControllerBase
    {
        readonly IGetAllStationNearLatLongPoint GetSTationNearLatLng;
        readonly IPetrolStationRepository PetrolStationRepository;

		public StationsController(IGetAllStationNearLatLongPoint getSTationNearLatLng, IPetrolStationRepository petrolStationRepository)
		{
			GetSTationNearLatLng = getSTationNearLatLng;
			PetrolStationRepository = petrolStationRepository;
		}


		//[HttpPost("refresh-token")]
		[AllowAnonymous]
        [HttpGet("get-nearest-stations")]
        public List<StationLite> GetNearestStations(double fromLat, double fromLongt, int countryId,
            DistanceUnit units)
        {
            return GetSTationNearLatLng.Execute(fromLat, fromLongt, countryId, units);
        }

		/*[AllowAnonymous]
		[HttpGet]
		public List<StationLite> All()
		{
			return PetrolStationRepository.GetAllFlat();
		}*/

	}
}
