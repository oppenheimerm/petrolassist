
using PA.Core.Models.ApiRequestResponse;
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
	public interface IAddPetrolStationUseCase
	{
		/// <summary>
		/// Add a new <see cref="Station"/> to the dataase.
		/// </summary>
		/// <param name="station"></param>
		/// <returns></returns>
		public Task<AddStationReplyRequest> ExecuteAsync(Station station);
	}
}
