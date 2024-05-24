
using System.ComponentModel.DataAnnotations;

namespace PA.Core.Models.ApiRequestResponse
{
	public class AddPetrolStationRequest
	{
		[Required]
		[StringLength(100)]
		[Display(Name = "Name")]
		public string StationName { get; set; } = string.Empty;

		[Required]
		[StringLength(200)]
		[Display(Name = "Address")]
		public string StationAddress { get; set; } = string.Empty;

		[Required]
		[StringLength(25)]
		[Display(Name = "Postcode")]
		public string StationPostcode { get; set; } = string.Empty;

		[Required]
		public double? Latitude { get; set; }

		[Required]
		public double? Longitude { get; set; }

		public bool StationOnline { get; set; } = false;

		[Required]
		public int? VendorId { get; set; }

		[Required]
		public int? CountryId { get; set; }

		public string? StationPhoto { get; set; }

		public bool PayAtPump { get; set; } = false;
		public bool PayByApp { get; set; } = false;

		public bool AccessibleToiletNearby { get; set; } = false;
		
	}
}
