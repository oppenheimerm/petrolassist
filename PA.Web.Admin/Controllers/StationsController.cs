using Microsoft.AspNetCore.Mvc;
using PA.Core.Models.ApiRequestResponse;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Controllers
{
    [Route("api/stations")]
    [ApiController]
    // [Authorize]
    public class StationsController : ControllerBase
    {
        private readonly IPetrolStationRepository PetrolStationsRepo;
        private readonly IGetAllCountriesUseCase GetAllCountriesUseCase;

        public StationsController(IPetrolStationRepository petrolStationRepository, IGetAllCountriesUseCase getAllCountriesUseCase)
        {
            PetrolStationsRepo = petrolStationRepository;
            GetAllCountriesUseCase = getAllCountriesUseCase;
        }


        [HttpGet("all")]
        public IQueryable<StationLite> GetStations(int? countryCode)
        {
            // nullable parameter, we want all.
            var stations = PetrolStationsRepo.GetAll(null);
            return stations;
        }

        [HttpGet("countries")]
        public IQueryable<Country> GetCountries()
        {
            return GetAllCountriesUseCase.Execute();
        }
    }
}
