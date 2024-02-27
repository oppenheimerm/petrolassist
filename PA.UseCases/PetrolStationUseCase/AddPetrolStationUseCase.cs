
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.PetrolStationUseCase
{
	public class AddPetrolStationUseCase : IAddPetrolStationUseCase
	{
		readonly IPetrolStationRepository PetrolStationRepository;

        public AddPetrolStationUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        public async Task<AddStationReplyRequest> ExecuteAsync(Station station)
        {
            var status =  await PetrolStationRepository.Add(station);
            if(status.Success)
            {
                return new AddStationReplyRequest()
                {
                    ErrorMessage = string.Empty,
                    Station = status.station,
                    Success = true
                };
            }
            else
            {
                return new AddStationReplyRequest()
                {
                    ErrorMessage = status.ErrorMessage,
                    Station = null,
                    Success = false
                };
            }
		}
    }
}
