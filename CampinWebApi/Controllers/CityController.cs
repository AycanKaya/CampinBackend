using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CityDTO;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private ICityService cityServie;
    
    public CityController(ICityService cityServie)
    {
        this.cityServie = cityServie;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cities = await cityServie.GetAllCities();
        return Ok(cities);
    }
    // add city
    [HttpPost]
    public async Task<IActionResult> Post(CreateCityDTO dto)
    {
        var newCity = await cityServie.AddCity(dto);
        return Ok(newCity);
    }
    
}