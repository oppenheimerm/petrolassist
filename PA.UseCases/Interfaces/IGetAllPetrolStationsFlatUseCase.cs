
using PA.Core.Helpers.Paging;
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsFlatUseCase
    {
		/// <summary>
		/// Usecase for returning a <see cref="PagedList"/> list of <see cref="StationLite"/>.
		/// </summary>
		/// <returns></returns>
		PagedList<StationLite> Execute(int? countryId, int? sortingOrder, PagingParameters pagingParameters);
    }
}
