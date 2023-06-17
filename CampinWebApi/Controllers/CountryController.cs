using CampinWebApi.Contracts;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController
{
    private ICountryService countryService;
    
    public CountryController(ICountryService countryService)
    {
        this.countryService = countryService;
    }
    
    [HttpGet]
    public async Task<List<string>> GetCountries()
    {
        return await countryService.GetCountries();
    }
    
    [HttpPost]
    public async Task<bool> AddCountry(string countryName)
    {
        return await countryService.AddCountry(countryName);
    }
}