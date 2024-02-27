
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{	
	public class GetPetrolStationByIdUseCase : IGetPetrolStationByIdUseCase
	{
		readonly IPetrolStationRepository PetrolStationRepository;

        public GetPetrolStationByIdUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public async Task<Station?> ExecuteAsync(int Id)
        {
            return await PetrolStationRepository.GetStationById(Id);

		}
    }
}
