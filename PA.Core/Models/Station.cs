
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace PA.Core.Models
{
	public class Station
	{
		public Station()
		{
			this.Added = DateTime.Now;
			this.StationIdentifier = Guid.NewGuid();
		}

		[Key]
		public int Id { get; set; }

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
		public Point? GeoLocation { get; set; }


		public bool StationOnline { get; set; } = false;

		[Required]
		public int? VendorId { get; set; }
		public Vendor? Vendor { get; set; }

		[Required]
		public int? CountryId { get; set; }


		public DateTime? Added { get; set; }

		public Guid StationIdentifier { get; private set; }

		public bool PayAtPump { get; set; } = false;
		public bool PayByApp { get; set; } = false;

		public bool AccessibleToiletNearby { get; set; } = false;

		public ICollection<StationRating> StationRatings { get; set; }

		public ICollection<CustomerHistory> CustomerHistory { get; set; }
	}
}
