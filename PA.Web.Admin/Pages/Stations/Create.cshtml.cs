using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite;
using PA.Core.Helpers;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;
using PA.Web.Admin.Helpers;

namespace PA.Web.Admin.Pages.Stations
{
    public class CreateModel : PageModel
    {
		readonly IAddPetrolStationUseCase AddPetrolStationUseCase;
        readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
		readonly IConfiguration Configuration;
        readonly IGetAllVendorsUseCase GetAllVendorsUseCase;

        //private readonly IGetCountryCodeByIdUseCase GetCountryCodeByIdUseCase;
        public List<Country>? Countries;
        public List<Vendor>? Vendors;
		[BindProperty]
		public AddPetrolStationRequest NewStation { get; set; } = default!;
		public string GoogleAPIKey { get; set; }
		public string GoogleMapId { get; set; }


		public CreateModel(IAddPetrolStationUseCase addPetrolStationUseCase, IGetAllCountriesUseCase getAllCountriesUseCase,
            IConfiguration configuration, IGetAllVendorsUseCase getAllVendorsUseCase)
        {
            AddPetrolStationUseCase = addPetrolStationUseCase;
            GetAllCountriesUseCase = getAllCountriesUseCase; 
            Configuration = configuration;
            GetAllVendorsUseCase = getAllVendorsUseCase;
			GoogleAPIKey = Configuration["GoogleServices:ApiKey"];
		}



		public void OnGet()
        {
            NewStation = new();
            Countries = GetAllCountriesUseCase.Execute().ToList();
            Vendors = GetAllVendorsUseCase.Execute().ToList();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || NewStation == null)
			{
				Countries = GetAllCountriesUseCase.Execute().ToList();
				Vendors = GetAllVendorsUseCase.Execute().ToList();
				return Page();
			}
			else
			{
				//	geometryFactory with srid equal to 4326 (WGS 84). This is the standard in cartography and GPS systems.
				var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
				var status = await AddPetrolStationUseCase.ExecuteAsync(ModelHelper.ToStation(NewStation, geometryFactory));
				if (status.Success!.Value)
				{
					return RedirectToPage("Details", new { id = status.Station!.Id });
				}
				else
				{
					Countries = GetAllCountriesUseCase.Execute().ToList();
					Vendors = GetAllVendorsUseCase.Execute().ToList();
					ModelState.AddModelError(string.Empty, status.ErrorMessage ?? "Please correct errors");
					return Page();
				}

			}
		}

	}
}
