
using PA.Core.Helpers.Paging;
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsFlatUseCase
    {
		/// <summary>
		/// Usecase for returning a list of all <see cref="StationLite"/>. Stations
		/// </summary>
		/// <returns></returns>
		PagedList<StationLite> Execute();
    }
}
