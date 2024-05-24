
using PA.Core.Models;
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
	public interface IGetStationByIdExternalUseCase
	{
		/// <summary>
		/// Get <see cref="StationLite"/> for external api request.  
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		Task<StationLite?> ExecuteAsync(Guid Id);
	}
}
