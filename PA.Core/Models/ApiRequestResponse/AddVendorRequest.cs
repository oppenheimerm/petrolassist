
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models.ApiRequestResponse
{
	public class AddVendorRequest
	{

		[Required]
		[StringLength(100)]
		[Display(Name = "Name")]
		public string VendorName { get; set; } = string.Empty;

		[Required]
		[StringLength(200)]
		[Display(Name = "Address")]
		public string VendorAddress { get; set; } = string.Empty;


		[Required]
		[StringLength(25)]
		[Display(Name = "Postcode")]
		public string VendorPostcode { get; set; } = string.Empty;


		[Required]
		[ForeignKey("Country")]
		public int? CountryId { get; set; }
		

		[Required]
		[StringLength(4, MinimumLength = 4)]
		public string VendorCode { get; set; } = string.Empty;

		[StringLength(40)]
		public string? VendorLogo { get; set; }

	}
}
