
using PA.Core.Helpers.Paging;
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllStationsUseCase
    {
		/// <summary>
		/// Returns a <see cref="PagedList"/> list of <see cref="Station"/>, including  related data.
		/// </summary>
		/// <param name="countryId"></param>
		/// <returns></returns>
		PagedList<Station> Execute(int? countryId, int? sortingOrder, PagingParameters pagingParameters);
    }
}
