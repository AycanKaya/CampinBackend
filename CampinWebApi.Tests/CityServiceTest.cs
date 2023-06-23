using CampinWebApi.Core.DTO.CityDTO;
using CampinWebApi.Domain;
using CampinWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CityServiceTest
{
    private readonly CampinDbContext _context;
    private readonly CityService _cityService;

    public CityServiceTest()
    {
        // Create options for DbContext instance
        var options = new DbContextOptionsBuilder<CampinDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Unique name for in-memory database
            .Options;

        // Create instance of DbContext
        _context = new CampinDbContext(options);

        // Create an instance of CityService with DbContext
        _cityService = new CityService(_context);
    }

    [Fact]
    public async Task AddCity_WhenCalled_ReturnsCity()
    {
        var cityDto = new CreateCityDTO
        {
            CountryId = "1",
            CityName = "CityTest"
        };

        var city = await _cityService.AddCity(cityDto);

        Assert.NotNull(city);
        Assert.Equal(cityDto.CityName, city.CityName);
    }

    // Add more tests for other methods here.
}