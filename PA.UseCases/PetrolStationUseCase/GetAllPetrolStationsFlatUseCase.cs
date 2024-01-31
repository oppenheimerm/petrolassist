using PA.Core.Helpers.Paging;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
    public class GetAllPetrolStationsFlatUseCase : IGetAllPetrolStationsFlatUseCase
    {
        private readonly IPetrolStationRepository PetrolStationRepository;

        public GetAllPetrolStationsFlatUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public PagedList<StationLite> Execute(int? countryId, int? sortingOrder, PagingParameters pagingParameters)
        {
            return PetrolStationRepository.GetAllFlat(countryId, sortingOrder, pagingParameters);
        }
    }
}
