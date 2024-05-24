
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;


namespace PA.UseCases.PetrolStationUseCase
{
	public class GetStationByIdExternalUseCase : IGetStationByIdExternalUseCase
	{
		readonly IPetrolStationRepository PetrolStationRepository;

		public GetStationByIdExternalUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public async Task<StationLite?> ExecuteAsync(Guid Id)
		{
			return await PetrolStationRepository.GetStationByGuid(Id);
		}
	}
}
