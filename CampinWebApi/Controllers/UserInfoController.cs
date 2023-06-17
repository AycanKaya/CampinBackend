using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CityDTO;
using CampinWebApi.Core.DTO.UserDTO;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]

public class UserInfoController : ControllerBase
{
    private  IUserInfoService userInfoService;
    
    public UserInfoController(IUserInfoService userInfoService)
    {
        this.userInfoService = userInfoService;
    }

    [HttpGet("GetUserInfo")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString();
            var cities = await userInfoService.GetUserInfo(token);
            return Ok(cities);
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Please log in" , (int)HttpStatusCode.Unauthorized);
            return new UnauthorizedObjectResult(response);        
        }
        
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }

    [HttpPost("EditUserInfo")]
    public async Task<IActionResult> Post(EditUserInfoDTO dto)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString();
            var newCity = await userInfoService.EditUserInfo(dto, token);
            return Ok(newCity);
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
            var response = new ErrorResponseModel(exception.Message,"You are not authorized!" , (int)HttpStatusCode.Unauthorized);
            return new UnauthorizedObjectResult(response);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
}