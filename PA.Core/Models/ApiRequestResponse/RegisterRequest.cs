
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PA.Core.Models.ApiRequestResponse
{
	public class RegisterRequest
	{

		[Required(ErrorMessage = "First name field is required")]
		[StringLength(30)]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name field is required")]
		[StringLength(30)]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "A valid email is required to register")]
		[EmailAddress]
		public string EmailAddress { get; set; } = string.Empty;

		[Required]
		[MinLength(6)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage ="Comfirm password is requires and must match.")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

		[Range(typeof(bool), "true", "true", ErrorMessage = "To register for this service, you must agree to our terms of service.")]
		public bool AcceptTerms { get; set; } = false;

		[Required(ErrorMessage ="A valid mobile phone number is required for this appplication.")]
		public string MobileNumber { get; set; } = string.Empty;
	}

	public static class RegisterRequestValidation
	{
		public static (List<String> Errors, bool FormValid) IsFormValid(RegisterRequest rqst)
		{

			List<String> errors = new List<String>();
			bool formValid = false;


			if (
				(string.IsNullOrEmpty(rqst.FirstName)  || rqst.FirstName == "string") ||
				(string.IsNullOrEmpty(rqst.LastName) || rqst.LastName == "string") ||
				(string.IsNullOrEmpty(rqst.EmailAddress) || rqst.EmailAddress == "string") ||
				(string.IsNullOrEmpty(rqst.Password) || rqst.Password == "string") ||
				(string.IsNullOrEmpty(rqst.ConfirmPassword) || rqst.ConfirmPassword == "string") ||
				(string.IsNullOrEmpty(rqst.MobileNumber) || rqst.MobileNumber == "string")
				)
			{
				errors.Add("All fields are required");
				return (errors, formValid);
			}
			else if (rqst.Password.Length < 6)
			{
				errors.Add("Password must be greater than 5 characters");
				return (errors, formValid);
			}
			else if (IsPasswordStrong(rqst.Password) == false)
			{
				errors.Add("Password must have as least one uppercase letter, at lease one lowercase letter, at least one digit and at least one special characte");
				return (errors, formValid);
			}
			else if (rqst.Password != rqst.ConfirmPassword)
			{
				errors.Add("Password and Confirm Password must match");
				return (errors, formValid);
			}
			else if (IsUKMobile(rqst.MobileNumber) == false)
			{
				errors.Add("Please eneter a valid UK mobile phone number");
				return (errors, formValid);
			}
			else
			{
				formValid = true;
				return (errors, formValid);
			}

		}


		//https://uibakery.io/regex-library/password-regex-csharp
		static bool IsPasswordStrong(string password)
		{
			Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$");
			return validateGuidRegex.IsMatch(password);
		}

		static bool IsUKMobile(string mobile)
		{
			Regex validUKMobile = new Regex("((\\+44(\\s\\(0\\)\\s|\\s0\\s|\\s)?)|0)7\\d{3}(\\s)?\\d{6}");
			return validUKMobile.IsMatch(mobile);
		}
	}
}
