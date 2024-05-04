using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using NetTopologySuite.Geometries;
using System.Drawing;

namespace PA.Core.Helpers
{
    public static class ModelHelper
    {
        /*public static StationLite ToStationLite(Station vm)
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
                    //GeoLocation = vm.GeoLocation,
                    Latitude = vm.GeoLocation.Coordinate.Y,
                    Longitude = vm.GeoLocation.Coordinate.X,
                    StationOnline = vm.StationOnline,
                    VendorName = vm.Vendor?.VendorName ?? string.Empty,
                    Logo = vm.Vendor?.VendorLogo ?? string.Empty,
                };
            }
        }*/

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
