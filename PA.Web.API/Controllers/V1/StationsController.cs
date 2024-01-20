using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;

namespace PA.Web.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization.Attributes.Authorize]
    public class StationsController : ControllerBase
    {
        //public IList<Station> Stations { get; set; } = default!;
        public readonly IGetAllPetrolStationsFlatUseCase GetAllPetrolStationsFlatUseCase;

        public StationsController(IGetAllPetrolStationsFlatUseCase getAllPetrolStationsFlatUseCase)
        {
            GetAllPetrolStationsFlatUseCase = getAllPetrolStationsFlatUseCase;
        }

        // GET: api/Stations
        [HttpGet]
        //[AllowAnonymous]
        public List<StationLite> GetStations()
        {
            return GetAllPetrolStationsFlatUseCase.Execute().ToList();
        }
    }
}
