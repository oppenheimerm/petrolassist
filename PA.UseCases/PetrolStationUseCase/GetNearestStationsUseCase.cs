
using Microsoft.AspNetCore.Mvc;
using PA.Core.Helpers.Paging;
using PA.Core.Helpers;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
    public class GetNearestStationsUseCase : IGetNearestStationsUseCase
	{
        private readonly IPetrolStationRepository PetrolStationRepository;

        public GetNearestStationsUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public List<StationLite> Execute(double fromLat, double fromLongt, DistanceUnit units)
        {
            return PetrolStationRepository.GetStationsNearLocation(fromLat, fromLongt, units);
        }
    }
}
