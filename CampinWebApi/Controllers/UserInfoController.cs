using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CityDTO;
using CampinWebApi.Core.DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;


[ApiController]
[Route("[controller]")]
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
        var token = HttpContext.Request.Headers.Authorization.ToString();
        var cities = await userInfoService.GetUserInfo(token);
        return Ok(cities);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
   
    }
    
}