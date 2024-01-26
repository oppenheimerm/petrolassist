using System.ComponentModel.DataAnnotations;

namespace PA.Web.Admin.ViewModels
{
    public class CoordinateRequest
    {

        public string? latitude { get; set; }

        public string? longitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /*public static double GetLatitude() {
            //  _lat will have the value of the parse "latitude", if parsing was succesful, otherwise it will
            //  have  the value of 0 declared here.
            double _lat = 0;
            if (latitude != null && !string.IsNullOrEmpty(latitude))
            {
                
                double.TryParse(latitude, out _lat);                 
            }
            return _lat;
        }

        public static double GeLongtitude()
        {
            //  _longt will have the value of the parse "longitude", if parsing was succesful, otherwise it will
            //  have  the value of 0 declared here.
            double _longt = 0;
            if (longitude != null && !string.IsNullOrEmpty(longitude))
            {
                double.TryParse(longitude, out _longt);
            }
            return _longt;
        }*/

    }
}
