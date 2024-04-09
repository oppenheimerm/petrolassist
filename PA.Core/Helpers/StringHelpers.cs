
using System.Diagnostics;
using System.Globalization;

namespace PA.Core.Helpers
{
	public static class StringHelpers
	{
		public const string VendorLogoUrlPrefix = "./img/assets/logos/";
		public const string ServerBaseUrl = "https://localhost:7038/";

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

		/// <summary>
		/// Helper methid to convert string to title case
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static string ConvertToTitleCase(this string target)
		{
			var lowerCase = target.ToLowerInvariant();
			var trimCase = lowerCase.Trim();

			// Creates a TextInfo based on the "en-US" culture.
			TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;

			return textInfo.ToTitleCase(trimCase);
		}
	}
}
