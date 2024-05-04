
using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers.Paging;
using PA.Core.Helpers;
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllStationNearLatLongPoint
    {
        List<StationLite> Execute(double fromLat, double fromLongt, int countryId,
                    DistanceUnit units);
    }
}
