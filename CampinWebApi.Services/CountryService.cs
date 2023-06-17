using CampinWebApi.Contracts;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CountryService : ICountryService
{
    private ICampinDbContext context;
    
    public CountryService(ICampinDbContext context)
    {
        this.context = context;
    }
    
    public async Task<List<string>> GetCountries()
    {
        var countries = await context.Countries
            .Select(c => c.CountryName)
            .ToListAsync();
        
        return countries;
    }
    
    //ad new country 
    public async Task<bool> AddCountry(string countryName)
    {
        var country = new Country
        {
            CountryName = countryName
        };
        
        await context.Countries.AddAsync(country);
        await context.SaveChanges();
        return true;
    }
    
}