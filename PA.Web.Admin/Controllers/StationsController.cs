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
        private readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
        private readonly IGetAllStationsUseCase GetAllStationsUseCase;

        public StationsController(IGetAllCountriesUseCase getAllCountriesUseCase, IGetAllStationsUseCase getAllStationsUseCase)
        {
            GetAllCountriesUseCase = getAllCountriesUseCase;
            GetAllStationsUseCase = getAllStationsUseCase;
        }


        [HttpGet("all")]
        public IQueryable<Station> GetStations(int? countryCode)
        {
            // nullable parameter, we want all.
            var stations = GetAllStationsUseCase.Execute(countryCode);
            return stations;
        }

        [HttpGet("countries")]
        public IQueryable<Country> GetCountries()
        {
            return GetAllCountriesUseCase.Execute();
        }
    }
}
