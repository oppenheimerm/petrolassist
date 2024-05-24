using NetTopologySuite.Geometries;
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace PA.UseCases.PetrolStationUseCase
{
	public class EditPetrolStationUseCase : IEditPetrolStationUseCase
	{
        readonly IPetrolStationRepository PetrolStationRepository;
        public EditPetrolStationUseCase(IPetrolStationRepository petrolStationRepository)
        {
            PetrolStationRepository = petrolStationRepository;
        }

        
        public async Task<(Station station, bool Success, string ErrorMessage)> ExecuteAsync(Station station)
        {
            var response = await PetrolStationRepository.Edit(station);
            return response;
        }
	}
}
