using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;
using PA.UseCases.PetrolStationUseCase;

namespace PA.Web.Admin.Pages.Vendors
{
    public class EditModel : PageModel
    {
		readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
		readonly IGetVendorByIdUseCase GetVendorByIdUseCase;
		readonly IEditVendorUseCase EditVendorUseCase;

		public List<Country>? Countries;
	

		[BindProperty]
		public Vendor? Vendor { get; set; }

		public EditModel(IGetAllCountriesUseCase getAllCountriesUseCase, IGetVendorByIdUseCase getVendorByIdUseCase,
            IEditVendorUseCase editVendorUseCase)
        {
			GetVendorByIdUseCase = getVendorByIdUseCase;
            GetAllCountriesUseCase = getAllCountriesUseCase;
            EditVendorUseCase = editVendorUseCase;

        }

		public async Task<IActionResult> OnGetAsync(int Id)
		{
			Vendor = await GetVendorByIdUseCase.ExecuteAsync(Id);
			Countries = GetAllCountriesUseCase.Execute().ToList();

			if (Vendor == null)
			{
				return NotFound();
			}


			return Page();
		}
	}
}
