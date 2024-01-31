
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetCountryByIdUseCase
    {
        Task<Country?> ExecuteAsync(int id);
    }
}
