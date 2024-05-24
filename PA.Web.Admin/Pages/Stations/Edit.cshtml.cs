using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PA.Core.Models;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Pages.Stations
{
    public class EditModel : PageModel
    {
        readonly IGetPetrolStationByIdUseCase GetPetrolStationByIdUseCase;
        readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
        readonly IGetAllVendorsUseCase GetAllVendorsUseCase;
        readonly IConfiguration Configuration;
        readonly IEditPetrolStationUseCase EditPetrolStationUseCase;



		public List<Country>? Countries;
        public List<Vendor>? Vendors;
        public string GoogleAPIKey { get; set; }
        public string GoogleMapId { get; set; }

        [BindProperty]
        public Station? Station { get; set; }

        public double? StationLat { get; set; }
        public double? StationLongt { get;set; }

        public EditModel(IGetPetrolStationByIdUseCase getPetrolStationByIdUseCase, IGetAllCountriesUseCase getAllCountriesUseCase,
            IGetAllVendorsUseCase getAllVendorsUseCase, IConfiguration configuration, IEditPetrolStationUseCase editPetrolStationUseCase)
        {
            GetPetrolStationByIdUseCase = getPetrolStationByIdUseCase;
            GetAllCountriesUseCase = getAllCountriesUseCase;
            GetAllVendorsUseCase = getAllVendorsUseCase;
            Configuration = configuration;
            GoogleAPIKey = Configuration?["GoogleServices:ApiKey"];
            EditPetrolStationUseCase = editPetrolStationUseCase;

        }
        public async Task<IActionResult> OnGetAsync(int Id)
        {
            Station = await GetPetrolStationByIdUseCase.ExecuteAsync(Id);
            Countries = GetAllCountriesUseCase.Execute().ToList();
            Vendors = GetAllVendorsUseCase.Execute().ToList();
            var hello = "HEllO";

            if(Station == null)
            {
                return NotFound();
            }

            if (Station?.GeoLocation != null)
            {
				StationLat = Station.GeoLocation.Coordinate.Y;
				StationLongt = Station.GeoLocation.Coordinate.X;
			}



			return Page();
        }


		public async Task<IActionResult> OnPostAsync()
        {
			var lat = Request.Form["LatEdit"];
            var longt = Request.Form["LongtEdit"];
            //var stationId = int.Parse(Request.Form["StationId"]);

           

			var updateLatLngStatus = UpdateLatLng(lat, longt);

            if(updateLatLngStatus == true)
            {
				//	geometryFactory with srid equal to 4326 (WGS 84). This is the standard in cartography and GPS systems.
				var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
				////  new Coordinate(long, lat)
				Station.GeoLocation = geometryFactory.CreatePoint(new Coordinate(StationLongt.Value, StationLat.Value));
                var status = await EditPetrolStationUseCase.ExecuteAsync(Station);
                if(status.Success == true)
                {
                    return RedirectToPage("/Stations/Index");
                }
                else
                {
					ModelState.AddModelError(string.Empty, status.ErrorMessage);
					return Page();
				}
            }
            else
            {
				ModelState.AddModelError(string.Empty, "Lattitude and Laongitude are reqquired. ");
				return RedirectToPage("./Index");
			}
			
		}


		private bool UpdateLatLng(string lat, string lng)
		{
            if(lat == null || lng == null) return false;
            else
            {
                double latOut, lngOut;
                if((double.TryParse(lat, out latOut) && (double.TryParse(lng, out lngOut)))){
                    StationLat = latOut; StationLongt = lngOut;
                    return true;
                }
                else
                {
                    return false;
                }
            }



			StationLat = double.Parse(lat);
			StationLongt = double.Parse(lng);

		}

	}
}
