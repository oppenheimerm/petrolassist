using System.ComponentModel.DataAnnotations;

namespace PA.Web.Admin.Models
{
	public class AdminLoginRequest
	{
		[Required]
		[Display(Name = "Username")]
		public string Email { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Password")]
		public string Password { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Pin")]
		public string Pin { get; set; } = string.Empty;
	}

}
