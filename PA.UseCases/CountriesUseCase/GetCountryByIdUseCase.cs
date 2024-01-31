
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.CountriesUseCase
{
    public class GetCountryByIdUseCase : IGetCountryByIdUseCase
    {
        readonly ICountryRepository CountryRepository;

        public GetCountryByIdUseCase( ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public async Task<Country?> ExecuteAsync(int id)
        {
            return await CountryRepository.GetCountryByIdAsync(id);
        }
    }
}
