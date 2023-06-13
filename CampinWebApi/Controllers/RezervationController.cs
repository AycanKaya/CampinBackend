using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Core.DTO.PaymentDTO;
using CampinWebApi.Core.DTO.RezervationDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RezervationController :ControllerBase
{
    private IRezervationService rezervationService;
    
    public RezervationController(IRezervationService rezervationService)
    {
        this.rezervationService = rezervationService;
    }
    
    [HttpPost("MakeRezervation")]
    public async Task<IActionResult> AddCustomerInfo(MakeRezervationDTO dto)
    {
        var userToken = HttpContext.Request.Headers.Authorization.ToString();
        var isRezervationSuccess = await this.rezervationService.MakeRezervations(dto, userToken);
        return Ok(isRezervationSuccess);
    }
    
    
    [HttpGet("GetUserRezervedCampsite")]
    public async Task<IActionResult> GetUserRezervedCampsite()
    {
        var userToken = HttpContext.Request.Headers.Authorization.ToString();
        var campsite = await this.rezervationService.GetUserRezervedCampsite(userToken);
        return Ok(campsite);
    }
}   