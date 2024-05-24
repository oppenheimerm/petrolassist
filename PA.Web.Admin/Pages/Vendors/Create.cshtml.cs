using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Core.Helpers;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.UseCases.Interfaces;


namespace PA.Web.Admin.Pages.Vendors
{
    public class CreateModel : PageModel
    {
		readonly IGetAllCountriesUseCase GetAllCountriesUseCase;
		readonly IAddVendorUseCase AddVendorUseCase;


		public List<Country>? Countries;

		[BindProperty]
		public AddVendorRequest NewVendow { get; set; } = default!;

		public CreateModel(IGetAllCountriesUseCase getAllCountriesUseCase, IAddVendorUseCase addVendorUseCase)
        {
            GetAllCountriesUseCase = getAllCountriesUseCase;
			AddVendorUseCase = addVendorUseCase;
        }

		public void OnGet()
        {
            NewVendow = new();
			Countries = GetAllCountriesUseCase.Execute().ToList();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || NewVendow == null)
			{
				Countries = GetAllCountriesUseCase.Execute().ToList();
				return Page();
			}
			else
			{
				var status = await AddVendorUseCase.ExecuteAsync(ModelHelper.ToVendor(NewVendow));

				if (status.Success)
				{
					return RedirectToPage($"./Index");
				}
				else
				{
					Countries = GetAllCountriesUseCase.Execute().ToList();
					ModelState.AddModelError(string.Empty, status.ErrorMessage ?? "Please correct errors");
					return Page();
				}
			}
		}

	}
}
