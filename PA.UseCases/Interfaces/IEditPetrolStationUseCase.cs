
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IEditPetrolStationUseCase
	{
		Task<(Station station, bool Success, string ErrorMessage)> ExecuteAsync(Station station);
	}
}
