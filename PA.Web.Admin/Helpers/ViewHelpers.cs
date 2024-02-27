using Newtonsoft.Json;
using PA.Core.Helpers;
using PA.Core.Models;

namespace PA.Web.Admin.Helpers
{
	public static class ViewHelpers
	{
		public const string VendorLogoUrlPrefix =  "../../img/assets/logos/";
		public const string ServerBaseUrl = "https://localhost:7038/";

		public static string GetVendorLogo(string logoName, VendorLogoSize size)
		{
			logoName = size switch
			{
				VendorLogoSize.ExtraSmall => VendorLogoUrlPrefix + logoName + "_logo_40_x_40.jpg",
				VendorLogoSize.Small => VendorLogoUrlPrefix + logoName + "_logo_80_x_80.jpg",
				VendorLogoSize.Medium => VendorLogoUrlPrefix + logoName + "_logo_100_x_100.jpg",
				VendorLogoSize.Large => VendorLogoUrlPrefix + logoName + "_logo_200_x_200.jpg",
				_ => throw new InvalidOperationException(),
			};

			return logoName;
		}

		public static string StationAsJson(Station station)
		{
			var vm = ModelHelper.ToStationLite(station);
			return JsonConvert.SerializeObject(vm);
		}
	}
}
