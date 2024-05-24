using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using NetTopologySuite.Geometries;


namespace PA.Core.Helpers
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
                    Id = vm.StationIdentifier.ToString(),
                    StationName = vm.StationName,
                    StationAddress = vm.StationAddress,
                    StationPostcode = vm.StationPostcode,
                    Latitude = vm.GeoLocation.Coordinate.Y,
                    Longitude = vm.GeoLocation.Coordinate.X,
                    StationOnline = vm.StationOnline,
                    VendorName = vm.Vendor?.VendorName ?? string.Empty,
                    Logo = vm.Vendor?.VendorLogo ?? string.Empty,
                    PayAtPump = vm.PayAtPump,
                    PayByApp = vm.PayByApp,
                    AccessibleToiletNearby = vm.AccessibleToiletNearby,
                };
            }
        }

        public static Station ToStation(AddPetrolStationRequest rqst, GeometryFactory geometryFactory)
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
					////  new Coordinate(long, lat)
					GeoLocation = geometryFactory.CreatePoint(new Coordinate(rqst.Longitude!.Value, rqst.Latitude!.Value)),
                    StationPhoto = rqst.StationPhoto,
                    StationOnline = rqst.StationOnline,
                    VendorId = rqst.VendorId,
                    CountryId = rqst.CountryId,
                    PayAtPump = rqst.PayAtPump,
                    PayByApp = rqst.PayByApp,
                    AccessibleToiletNearby = rqst.AccessibleToiletNearby,
                };
            }
        }

        public static Vendor ToVendor(AddVendorRequest request)
        {
            if (request == null) {
				throw new ArgumentNullException(nameof(request));
            }
            else
            {
                return new Vendor
                {
                    VendorName = request.VendorName,
                    VendorAddress = request.VendorAddress,
                    VendorPostcode = request.VendorPostcode,
                    CountryId = request.CountryId,
                    VendorCode = request.VendorCode,
                    VendorLogo = request.VendorLogo
                };
            }
        }
    }
}
