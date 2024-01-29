
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

        public IQueryable<Station> Execute(int? countryId)
        {
            return PetrolStationRepository.GetAll(countryId);
        }
    }
}
