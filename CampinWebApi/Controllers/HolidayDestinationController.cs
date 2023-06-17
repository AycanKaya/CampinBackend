using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.HolidayDestinationDTO;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("[controller]")]
[ApiController]

public class HolidayDestinationController :ControllerBase
{
    private IHolidayDestinationService holidayDestinationService;
    
    public HolidayDestinationController(IHolidayDestinationService holidayDestinationService)
    {
        this.holidayDestinationService = holidayDestinationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var holidayDestinations = await holidayDestinationService.GetAllHolidayDestinations();
        return Ok(holidayDestinations);
    }
    // create holiday destination
    [HttpPost]
    public async Task<IActionResult> Post(AddHolidayDestinationDTO holidayDestinationDto)
    {
        var createdHolidayDestination = await holidayDestinationService.AddHolidayDestination(holidayDestinationDto);
        return Ok(createdHolidayDestination);
    }
    
}