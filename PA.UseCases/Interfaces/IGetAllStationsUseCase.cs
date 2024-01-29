
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllStationsUseCase
    {
        /// <summary>
        /// Returns a IQuerable list of <see cref="Station"/>, including  related data.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        IQueryable<Station> Execute(int? countryId);
    }
}
