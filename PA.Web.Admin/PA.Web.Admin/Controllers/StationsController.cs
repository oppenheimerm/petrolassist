using Microsoft.AspNetCore.Mvc;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Web.Admin.Controllers
{
	[Route("stations")]
	[ApiController]
	// [Authorize]
	public class StationsController : ControllerBase
	{
		private readonly IPetrolStationRepository PetrolStationsRepo;

        public StationsController(IPetrolStationRepository petrolStationRepository)
        {
			PetrolStationsRepo = petrolStationRepository;
        }


        [HttpGet("all")]
		public IQueryable<StationLite> GetStations(int? countryCode)
		{
			// nullable parameter, we want all.
			var stations = PetrolStationsRepo.GetAll(null);
			return stations;
		}
	}
}
