
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PA.Core.Helpers;
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;

namespace PA.Datastore.EFCore.Repositories
{
    public class PetrolStationRepository : IPetrolStationRepository
    {
        private readonly ILogger<PetrolStationRepository> Logger;
        private readonly ApplicationManagmentDbContext Context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public PetrolStationRepository(ILogger<PetrolStationRepository> logger, ApplicationManagmentDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            Logger = logger;
            Context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<(Station station, bool Success, string ErrorMessage)> Add(Station station)
        {
            // PostCode exist
            var postCodeInUse = await PostCodeInUseAsync(station.StationPostcode);
            if (postCodeInUse)
            {
                var errorMsg = $"Attempted to add station with duplicate postcode: {station.StationPostcode}. Timestamp: {DateTime.UtcNow}";
                Logger.LogError(errorMsg);
                return (new Station(), false, errorMsg);
            }
            try
            {
                station.StationPostcode = station.StationPostcode.ToUpperInvariant();
                Context.PetrolStations.Add(station);
                await Context.SaveChangesAsync();
                Logger.LogInformation($"Station with Id: {station.Id}, and name of: {station.StationName} added to database at: {DateTime.UtcNow}");
                return (station, true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add station to database. Timestamp : {DateTime.UtcNow}");
                return (station, false, ex.ToString());
            }
        }

        public async Task<(Station station, bool success, string ErrorMessage)> Edit(Station station)
        {
            try
            {
                station.StationPostcode = station.StationPostcode.ToUpperInvariant();
                Context.PetrolStations.Update(station);

				Context.Entry(station).State = EntityState.Modified;
				await Context.SaveChangesAsync();
                Logger.LogInformation($"Station with Id: {station.Id}, and name of: {station.StationName} was updated at: {DateTime.UtcNow}");
                return (station, true, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to update station: {station.StationName}. Timestamp : {DateTime.UtcNow}");
                return (station, false, ex.ToString());
            }

        }

        public PagedList<Station> GetAll(int? countryId, int? sortingOrder, PagingParameters pagingParameters) {

			// Get Enum sort type
			var sortOrder = sortingOrder.HasValue ? PaginHelpers.GetStationSortOrder(sortingOrder.Value) : StationSortOrder.Id;

			if (countryId == null )
            {

				var stations = Context.PetrolStations
					.Include(v => v.Vendor)
					.Include(r => r.StationRatings)
					.Include(h => h.StationRatings).AsNoTracking();

				// nullable parameter, we want all.
				switch (sortOrder)
				{
					case StationSortOrder.Id:
                        stations = stations.OrderBy(i => i.Id);
						break;
					case StationSortOrder.StationName:
						stations = stations.OrderBy(n => n.StationName);
						break;
					case StationSortOrder.AddedDate:
						stations = stations.OrderBy(d => d.Added);
						break;
					case StationSortOrder.StationPostCode:
						stations = stations.OrderBy(p => p.StationPostcode);
						break;
					default:
						stations = stations.OrderBy(i => i.Id);
						break;
				}

                //return stations;
                return PagedList<Station>.ToPagedList(stations, pagingParameters.PageNumber, pagingParameters.PageSize);

			}
            else
            {
				var stations = Context.PetrolStations
					.Where(s => s.CountryId == countryId)
					.Include(v => v.Vendor)
					.Include(r => r.StationRatings)
					.Include(h => h.StationRatings).AsNoTracking();


				// nullable parameter, we want all.
				switch (sortOrder)
				{
					case StationSortOrder.Id:
						stations = stations.OrderBy(i => i.Id);
						break;
					case StationSortOrder.StationName:
						stations = stations.OrderBy(n => n.StationName);
						break;
					case StationSortOrder.AddedDate:
						stations = stations.OrderBy(d => d.Added);
						break;
					case StationSortOrder.StationPostCode:
						stations = stations.OrderBy(p => p.StationPostcode);
						break;
					default:
						stations = stations.OrderBy(i => i.Id);
						break;
				}

				//return stations;
				return PagedList<Station>.ToPagedList(stations, pagingParameters.PageNumber, pagingParameters.PageSize); 
			}
            
        }

		/*public List<StationLite> GetAllFlat()
		{
			var query = from station in Context.PetrolStations
						join country in Context.Countries on station.CountryId equals country.Id
						join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
						select new StationLite
						{
							Id = station.Id,
							StationName = station.StationName,
							StationAddress = station.StationAddress,
							StationPostcode = station.StationPostcode,
                            Latitude = station.GeoLocation.Coordinate.Y,
                            Longitude = station.GeoLocation.Coordinate.X,
							//GeoLocation = station.GeoLocation,
							StationOnline = station.StationOnline,
							VendorName = vendor.VendorName,
							Country = country.CountryName,
							Logo = vendor.VendorLogo,
							PayAtPump = station.PayAtPump,
							PayByApp = station.PayByApp,
							AccessibleToiletNearby = station.AccessibleToiletNearby,
							Added = station.Added
						};

            return query.ToList();
		}*/

		/*public PagedList<StationLite> GetAllFlat(int? countryId, int? sortingOrder, PagingParameters pagingParameters)
        {
			// Get Enum sort type
			var sortOrder = sortingOrder.HasValue ? PaginHelpers.GetStationSortOrder(sortingOrder.Value) : StationSortOrder.Id;

			if (countryId == null) {
                var query = from station in Context.PetrolStations
                            join country in Context.Countries on station.CountryId equals country.Id
                            join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                            select new StationLite
                            {
                                Id = station.Id,
                                StationName = station.StationName,
                                StationAddress = station.StationAddress,
                                StationPostcode = station.StationPostcode,
                                Latitude = station.GeoLocation.Coordinate.Y,
                                Longitude = station.GeoLocation.Coordinate.X,
                                //GeoLocation = station.GeoLocation,
                                StationOnline = station.StationOnline,
                                VendorName = vendor.VendorName,
                                Country = country.CountryName,
                                Logo = vendor.VendorLogo,
                                PayAtPump = station.PayAtPump,
                                PayByApp = station.PayByApp,
                                AccessibleToiletNearby = station.AccessibleToiletNearby,
                                Added = station.Added
							};


				switch (sortOrder)
				{
					case StationSortOrder.Id:
						query = query.OrderBy(i => i.Id);
						break;
					case StationSortOrder.StationName:
						query = query.OrderBy(n => n.StationName);
						break;
					case StationSortOrder.AddedDate:
						query = query.OrderBy(d => d.Added);
						break;
					case StationSortOrder.StationPostCode:
						query = query.OrderBy(p => p.StationPostcode);
						break;
					default:
						query = query.OrderBy(i => i.Id);
						break;
				}

				//return query;
				return PagedList<StationLite>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
			}
            else {
				var query = from station in Context.PetrolStations
							join country in Context.Countries on station.CountryId equals country.Id
							join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
							where country.Id == countryId
							select new StationLite
							{
								Id = station.Id,
								StationName = station.StationName,
								StationAddress = station.StationAddress,
								StationPostcode = station.StationPostcode,
                                Latitude = station.GeoLocation.Coordinate.Y,
                                Longitude = station.GeoLocation.Coordinate.X,
                                //GeoLocation = station.GeoLocation,
								StationOnline = station.StationOnline,
								VendorName = vendor.VendorName,
								Country = country.CountryName,
								Logo = vendor.VendorLogo,
								PayAtPump = station.PayAtPump,
								PayByApp = station.PayByApp,
								AccessibleToiletNearby = station.AccessibleToiletNearby
							};

				switch (sortOrder)
				{
					case StationSortOrder.Id:
						query = query.OrderBy(i => i.Id);
						break;
					case StationSortOrder.StationName:
						query = query.OrderBy(n => n.StationName);
						break;
					case StationSortOrder.AddedDate:
						query = query.OrderBy(d => d.Added);
						break;
					case StationSortOrder.StationPostCode:
						query = query.OrderBy(p => p.StationPostcode);
						break;
					default:
						query = query.OrderBy(i => i.Id);
						break;
				}

				//return query;
				return PagedList<StationLite>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
			}

		}*/


        // Paged nearest stations using HaversineDistance, without paginh
        public List<StationLite> GetStationsNearLocation(double fromLat, double fromLongt, DistanceUnit units)

        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
			var usersLocation = geometryFactory.CreatePoint(new Coordinate(fromLongt, fromLat));
            //double maxDistance = 0.0;
            


			var query = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        orderby station.GeoLocation.IsWithinDistance(usersLocation, 20000)/* 20km */
                        select new StationLite
                        {
                            Id = station.StationIdentifier.ToString(),
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
                            StationPostcode = station.StationPostcode,
                            Latitude = station.GeoLocation.Coordinate.Y,
                            Longitude = station.GeoLocation.Coordinate.X,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.VendorLogo,
                            PayAtPump = station.PayAtPump,
                            PayByApp = station.PayByApp,
                            SIUnit = units,
                            AccessibleToiletNearby = station.AccessibleToiletNearby,
                            Distance = (units == DistanceUnit.Kilometers) ? ( (station.GeoLocation.Distance(usersLocation)) / 1000)  : ((station.GeoLocation.Distance(usersLocation)) * 0.00062137)
						};


            
            
            

            var stations = query
                .OrderBy(s => s.Distance)
                .Where(d => d.Distance <= 20.00)
                .Take(20)
                .ToList();


            //Logger.LogInformation($"Found {stations.Count} near user.");

            return stations;

        }

        //  WORKS But want same without paging
        // Paged nearest stations using HaversineDistance
        /*public PagedList<StationLite> GetAllStationsNearLocation(double fromLat, double fromLongt, int countryId,
            DistanceUnit units, [FromQuery] PagingParameters pagingParms)
        {
            var query = from station in Context.PetrolStations
                        join country in Context.Countries on station.CountryId equals country.Id
                        join vendor in Context.PetrolVendors on station.VendorId equals vendor.Id
                        where country.Id == countryId
                        select new StationLite
                        {
                            Id = station.Id,
                            StationName = station.StationName,
                            StationAddress = station.StationAddress,
                            StationPostcode = station.StationPostcode,
                            Latitude = station.Latitude,
                            Longitude = station.Longitude,
                            StationOnline = station.StationOnline,
                            VendorName = vendor.VendorName,
                            Country = country.CountryName,
                            Logo = vendor.VendorLogo,
                            PayAtPump = station.PayAtPump,
                            PayByApp = station.PayByApp,
                            AccessibleToiletNearby = station.AccessibleToiletNearby
                        };*/

        //  Execution of the query is deferred until the query variable is iterated over in a foreach,
        //  For Each loop or ToList(). 



        /*return PagedList<Owner>.ToPagedList(FindAll().OrderBy(on => on.Name),
            ownerParameters.PageNumber,
            ownerParameters.PageSize);*/

        /*var stations = PagedList<StationLite>.ToPagedList(query, pagingParms.PageNumber, pagingParms.PageSize);
        for (int i = 0; i < stations.Count; i++)
        {

            stations[i].Distance = Math.Round(GeoHelpers.HaversineDistance(fromLat, fromLongt, stations[i], units), 2);
        }
        if (stations != null)
        {
            Logger.LogInformation($" Returned {stations.Count} items for query near this geo location at: {DateTime.UtcNow}");
        }
        else
        {
            Logger.LogInformation($" Could not find any locations near this geolocation. TimeStampe: {DateTime.UtcNow}");
        }
        return stations;
    }*/


        

        public async Task<Station?> GetStationById(int id)
        {
            return await Context.PetrolStations
                .Include(v => v.Vendor)
                .Include(r => r.StationRatings)
                .Include(h => h.CustomerHistory)
                .AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

		public async Task<StationLite?> GetStationByGuid(Guid id) {
            var station =  await Context.PetrolStations.Where(s => s.StationIdentifier == id).FirstAsync();
            return ModelHelper.ToStationLite(station);
        }

		public async Task<bool> PostCodeInUseAsync(string postCode)
        {
            // PostCodes are stored in uppercase.
            var postCodeToCompare = postCode.ToUpperInvariant();

            var exist = await Context.PetrolStations
                .Where(s => s.StationPostcode == postCodeToCompare)
                .AsNoTracking().FirstOrDefaultAsync();


            if (exist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

	}
}
