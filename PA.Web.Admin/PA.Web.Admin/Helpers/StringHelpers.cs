namespace PA.Web.Admin.Helpers
{
	public enum VendorLogoSize
	{
		ExtraSmall,
		Small,
		Medium,
		Large,
	}

	public static class StringHelpers
	{
		public const string VendorLogoUrlPrefix = "./img/assets/logos/";
		public const string ServerBaseUrl = "https://localhost:7029/";

		/// <summary>
		/// Truncate a string to a set size.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		/// public static string Truncate(this string value, int maxLength)
		public static string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="maxlen"></param>
		/// <returns></returns>
		/// public static string ShortenAndFormatText(this string val, int maxlen) with trailing ...
		public static string ShortenAndFormatText(string val, int maxlen)
		{
			const string postFix = "...";

			if (string.IsNullOrEmpty(val)) return val;

			if (val.Length > maxlen)
			{
				var truncateFirst = Truncate(val, (maxlen - postFix.Length));
				var truncateLast = truncateFirst + postFix;
				return truncateLast;
			}
			else
			{
				return val;
			}
		}

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

		public static string getLogoUrl(string image)
		{
			return $"./img/vendorlogos/{image}";
		}

		public static List<string> GetLogosForStation(string logo)
		{
			//"C:\\Users\\moppenheimer\\repo\\web\\PS\\PS.API\\wwwroot\\img\\logosasda_logo_80_x_80.jpg",
			//var path2 = $"/img/logos/{}";

			List<string> Logos = new List<string>();
			//var basePath = Path.Combine(WebHostingEnvironment.WebRootPath, "img", "logos");

			var ExtraSmall = $"{GetVendorLogo(logo, VendorLogoSize.ExtraSmall)}";
			Logos.Add(ExtraSmall);

			var Small = $"{GetVendorLogo(logo, VendorLogoSize.Small)}";
			Logos.Add(Small);

			var Medium = $"{GetVendorLogo(logo, VendorLogoSize.Medium)}";
			Logos.Add(Medium);

			var Large = $"{GetVendorLogo(logo, VendorLogoSize.Large)}";
			return Logos;
		}


	}
}
