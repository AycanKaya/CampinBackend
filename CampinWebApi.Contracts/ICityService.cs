using CampinWebApi.Core.DTO.CityDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICityService
{
    Task<City> AddCity(CreateCityDTO dto);
    Task<City> UpdateCity(City city);
    Task<City> DeleteCity(int id);
    Task<IEnumerable<City>> GetAllCities();
    
}