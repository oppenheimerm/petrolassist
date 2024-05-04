// SEE PA.Core.Helpers.ModelHelpers.cs


/*using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;


namespace PA.Web.Admin.Helpers
{
	public static class ModelHelper
	{
		public static StationLite ToStationLite(Station vm)
		{
			if (vm == null)
			{
				throw new ArgumentNullException(nameof(vm));
			}
			else
			{
				return new StationLite
				{
					Id = vm.Id,
					StationName = vm.StationName,
					StationAddress = vm.StationAddress,
					StationPostcode = vm.StationPostcode,
					Added = vm.Added,
					Latitude = vm.Latitude,
					Longitude = vm.Longitude,
					StationOnline = vm.StationOnline,
					VendorName = vm.Vendor?.VendorName ?? string.Empty,
					Logo = vm.Vendor?.VendorLogo ?? string.Empty,
				};
			}
		}

		public static Station ToStation(AddPetrolStationRequest rqst)
		{
			if (rqst == null)
			{
				throw new ArgumentNullException(nameof(rqst));
			}
			else
			{
				return new Station
				{
					StationName = rqst.StationName,
					StationAddress = rqst.StationAddress,
					StationPostcode = rqst.StationPostcode,
					Latitude = rqst.Latitude,
					Longitude = rqst.Longitude,
					StationOnline = rqst.StationOnline,
					VendorId = rqst.VendorId,
					CountryId = rqst.CountryId,
					PayAtPump = rqst.PayAtPump,
					PayByApp = rqst.PayByApp,
					AccessibleToiletNearby = rqst.AccessibleToiletNearby,
				};
			}
		}
	}
}
*/