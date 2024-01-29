
using PA.Core.Models.ApiRequestResponse;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllPetrolStationsFlatUseCase
    {
        /// <summary>
        /// Usecase for returning a IQuerable list of <see cref="StationLite"/>.
        /// </summary>
        /// <returns></returns>
        IQueryable<StationLite> Execute(int? countryId);
    }
}
