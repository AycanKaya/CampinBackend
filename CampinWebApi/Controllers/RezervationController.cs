using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Core.DTO.PaymentDTO;
using CampinWebApi.Core.DTO.RezervationDTO;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("[controller]")]
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
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var isRezervationSuccess = await this.rezervationService.MakeRezervations(dto, userToken);
            return Ok(isRezervationSuccess);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
    
    [HttpGet("GetUserRezervedCampsite")]
    public async Task<IActionResult> GetUserRezervedCampsite()
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var campsite = await this.rezervationService.GetUserRezervedCampsite(userToken);
            return Ok(campsite);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
}   