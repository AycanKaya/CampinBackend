using CampinWebApi.Services;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

public class CountryServiceTest
{
    private readonly CampinDbContext _context;
    private readonly CountryService _countryService;

    public CountryServiceTest()
    {
        // Create options for DbContext instance
        var options = new DbContextOptionsBuilder<CampinDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Unique name for in-memory database
            .Options;

        // Create instance of DbContext
        _context = new CampinDbContext(options);

        // Create an instance of CountryService with DbContext
        _countryService = new CountryService(_context);
    }

    [Fact]
    public async Task AddCountry_WhenCalled_ReturnsTrue()
    {
        var countryName = "TestCountry";

        var result = await _countryService.AddCountry(countryName);

        Assert.True(result);

        var country = await _context.Countries.SingleOrDefaultAsync(c => c.CountryName == countryName);
        Assert.NotNull(country);
        Assert.Equal(countryName, country.CountryName);
    }

    [Fact]
    public async Task GetCountries_WhenCalled_ReturnsCountryList()
    {
        var countryName1 = "TestCountry1";
        var countryName2 = "TestCountry2";
        await _countryService.AddCountry(countryName1);
        await _countryService.AddCountry(countryName2);

        var countries = await _countryService.GetCountries();

        Assert.Contains(countryName1, countries);
        Assert.Contains(countryName2, countries);
    }

    // You can add more tests for other scenarios here.
}
