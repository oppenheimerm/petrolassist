using PA.Core.Helpers.Paging;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
    public class GetAllPetrolStationsFlatUseCase : IGetAllPetrolStationsFlatUseCase
    {
        private readonly IGetAllPetrolStationsFlatUseCase PetrolStationsAllUC;

        public GetAllPetrolStationsFlatUseCase(IGetAllPetrolStationsFlatUseCase getAllPetrolStationsFlatUseCase)
        {
			PetrolStationsAllUC = getAllPetrolStationsFlatUseCase;

		}

        public PagedList<StationLite> Execute()
        {
            return PetrolStationsAllUC.Execute();
        }
    }
}
