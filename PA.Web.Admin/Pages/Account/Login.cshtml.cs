using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Web.Admin.Models;

namespace PA.Web.Admin.Pages.Account
{
    public class LoginModel : PageModel
    {
        public LoginModel()
        {
            
        }

        [BindProperty]
		public AdminLoginRequest LoginRequest { get; set; } = default!;

		public void OnGet()
        {
            LoginRequest = new();

		}
    }
}
