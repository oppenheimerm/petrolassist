using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;
using PA.Web.Admin.Helpers;

namespace PA.Web.Admin.Pages.Stations
{
    public class CreateModel : PageModel
    {
		IAddPetrolStationUseCase AddPetrolStationUseCase { get; set; }
		IGetAllCountriesUseCase GetAllCountriesUseCase { get; set; }
		IConfiguration Configuration { get; set; }
		IGetAllVendorsUseCase GetAllVendorsUseCase { get; set; }
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
				var status = await AddPetrolStationUseCase.ExecuteAsync(ModelHelper.ToStation(NewStation));
				if (status.Success!.Value)
				{
					return RedirectToPage($"Stations/Details/{status.Station!.Id}");
				}
				else
				{
					Countries = GetAllCountriesUseCase.Execute().ToList();
					Vendors = GetAllVendorsUseCase.Execute().ToList();
					return Page();
				}

			}
		}

	}
}
