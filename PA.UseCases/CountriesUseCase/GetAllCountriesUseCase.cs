using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.CountriesUseCase
{
    public class GetAllCountriesUseCase : IGetAllCountriesUseCase
    {
        private readonly ICountryRepository CountryRepository;

        public GetAllCountriesUseCase(ICountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }

        public IQueryable<Country> Execute()
        {
            return CountryRepository.GetAll();
        }
    }
}
