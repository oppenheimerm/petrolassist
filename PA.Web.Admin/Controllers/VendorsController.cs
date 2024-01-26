using Microsoft.AspNetCore.Mvc;
using PA.Core.Models;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Controllers
{
    [Route("api/vendors")]
    [ApiController]
    // [Authorize]
    public class VendorsController
    {
        private readonly IGetAllVendorsUseCase GetAllVendorsUseCase;
        public VendorsController(IGetAllVendorsUseCase getAllVendorsUse)
        {
            GetAllVendorsUseCase = getAllVendorsUse;
        }

        /*

        [HttpGet("all")]
        public IQueryable<StationLite> GetStations(int? countryCode)
        {
            // nullable parameter, we want all.
            var stations = PetrolStationsRepo.GetAll(null);
            return stations;
        }
         */

        [HttpGet("all")]
        public IQueryable<Vendor> GetVendors()
        {
            var vendors = GetAllVendorsUseCase.Execute();
            return vendors;
        }
    }
}
