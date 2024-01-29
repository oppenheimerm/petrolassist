using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Core.Models;
using PA.UseCases.Interfaces;

namespace PA.Web.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization.Attributes.Authorize]
    public class StationsController : ControllerBase
    {
        readonly IGetAllStationsUseCase GetPetrolStationsUseCase;

        public StationsController(IGetAllStationsUseCase getAllStationsUseCase)
        {
            GetPetrolStationsUseCase = getAllStationsUseCase;
        }

        // GET: api/Stations
        [HttpGet]
        //[AllowAnonymous]
        public IQueryable<Station> GetStations(int? countryId)
        {
            return GetPetrolStationsUseCase.Execute(countryId);
        }
    }
}
