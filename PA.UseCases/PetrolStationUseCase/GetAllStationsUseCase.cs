
using PA.Core.Helpers.Paging;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
    public class GetAllStationsUseCase : IGetAllStationsUseCase
    {
        readonly IPetrolStationRepository PetrolStationRepository;

        public GetAllStationsUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public PagedList<Station> Execute(int? countryId, int? sortingOrder, PagingParameters pagingParameters)
        {
            return PetrolStationRepository.GetAll(countryId, sortingOrder, pagingParameters);
        }
    }
}
