
namespace PA.Core.Helpers
{
    public enum DistanceUnit { Kilometers, Miles };

    public static class GeoHelpers
    {
        public static double GetMiles(double kilometers)
        {
            return (kilometers / 1.6);
        }

        public static double GetKilometers(double miles)
        {
            return (miles * 1.6);
        }
    }
}