using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Models;
using PA.UseCases.CountriesUseCase;
using PA.UseCases.Interfaces;

namespace PA.Web.Admin.Pages.Stations
{
	public class DetailsModel : PageModel
	{
		readonly IGetPetrolStationByIdUseCase GetPetrolStationByIdUseCase;
		readonly IGetCountryByIdUseCase GetCountryByIdUseCase;
		readonly IConfiguration Configuration;

		public Station? Station { get; set; }
		public Country? Country { get; set; }
		public string GoogleAPIKey { get; set; }
		public string GoogleMapId { get; set; }

		public DetailsModel(IGetPetrolStationByIdUseCase getPetrolStationByIdUseCase,
			IGetCountryByIdUseCase getCountryByIdUseCase,
			IConfiguration configuration)
		{
			GetPetrolStationByIdUseCase = getPetrolStationByIdUseCase;
			GetCountryByIdUseCase = getCountryByIdUseCase;
			Configuration = configuration;
		}

		public async Task<IActionResult> OnGetAsync(int Id)
		{
			var query = await GetPetrolStationByIdUseCase.ExecuteAsync(Id);
			var queryCountry = await GetCountryByIdUseCase.ExecuteAsync(query.CountryId.Value);
			GoogleAPIKey = Configuration["GoogleServices:ApiKey"];
			GoogleMapId = Configuration["GoogleServices:MapId"];


			if (query == null)
			{
				return NotFound();
			}
			else
			{
				Station = query;
				Country = queryCountry;
			}

			return Page();
		}
	}
}
