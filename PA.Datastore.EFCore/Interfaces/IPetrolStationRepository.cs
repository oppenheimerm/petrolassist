using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using System.Linq;

namespace PA.Datastore.EFCore.Interfaces
{
    public interface IPetrolStationRepository
    {
        /// <summary>
        /// Add a <see cref="Station"/> to the database
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        Task<(Station station, bool Success, string ErrorMessage)> Add(Station station);
        /// <summary>
        /// Edit / Update a <see cref="Station"/> entry.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        Task<(Station station, bool success, string ErrorMessage)> Edit(Station station);

		/// <summary>
		/// Returns <see cref="PagedList"/> collection of <see cref="Station"/> with sort and paging functionality
		/// </summary>
		/// <param name="countryId"></param>
		/// <param name="sortingOrder"></param>
		/// <param name="pagingParameters"></param>
		/// <returns></returns>
		PagedList<Station> GetAll(int? countryId, int? sortingOrder, PagingParameters pagingParameters);

        //  TODO - Removoe from production, for testing only
		/// <summary>
		/// Returns  <see cref="PagedList"/>y collection of <see cref="StationLite"/> with sort and paging functionality
		
		/// <returns></returns>
		//List<StationLite> GetAllFlat();
		/// <summary>
		/// Returns a <see cref="PagedList{T}"/> of <see cref="StationLite"/> objects ordered by distance
		/// </summary>
		/// <param name="fromLat"></param>
		/// <param name="fromLongt"></param>
		/// <param name="countryId"></param>
		/// <param name="units"></param>
		/// <returns></returns>
		/// 

		//public List<StationLite> GetStationsNearLocation(double fromLat, double fromLongt, int countryId,
		//    DistanceUnit units);

		/// <summary>
		/// Get stations near user. Not paged.
		/// </summary>
		/// <param name="fromLat"></param>
		/// <param name="fromLongt"></param>
		/// <param name="countryId"></param>
		/// <param name="units"></param>
		/// <returns></returns>
		List<StationLite> GetStationsNearLocation(double fromLat, double fromLongt, int countryId,
					DistanceUnit units);
		/// <summary>
		/// Get <see cref="Station"/> by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Station?> GetStationById(int id);
    }
}
