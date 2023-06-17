namespace CampinWebApi.Contracts;

public interface ICountryService
{
    Task<bool> AddCountry(string countryName);
    Task<List<string>> GetCountries();
}