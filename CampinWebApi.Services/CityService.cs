using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CityDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CityService : ICityService
{
    private readonly CampinDbContext context; 
    public CityService(CampinDbContext context)
    {
        this.context = context;
    }
    
    // add city
    public async Task<City> AddCity(CreateCityDTO dto)
    {
        var city = new City
        {
            CountryId = dto.CountryId,
            CityName = dto.CityName
        };
        
        await context.Cities.AddAsync(city);
        await context.SaveChangesAsync();
        return city;
    }
    // update city
    public async Task<City> UpdateCity(City city)
    {
        context.Cities.Update(city);
        await context.SaveChangesAsync();
        return city;
    }
    // delete city
    public async Task<City> DeleteCity(int id)
    {
        var city = await context.Cities.FindAsync(id);
        context.Cities.Remove(city);
        await context.SaveChangesAsync();
        return city;
    }
    // get all cities
    public async Task<IEnumerable<City>> GetAllCities()
    {
        return await context.Cities.ToListAsync();
    }
}