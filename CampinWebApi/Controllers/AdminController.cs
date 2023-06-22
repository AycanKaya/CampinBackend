using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService adminService;
    
    public AdminController(IAdminService adminService)
    {
        this.adminService = adminService;
    }
    
    [HttpDelete("deleteCampsite/{id}")]
    public async Task<IActionResult> DeleteCampsite(string id)
    {
        try
        {
            var result = await adminService.DeleteCampsite(id);
            return new OkObjectResult(result);
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
    
    [HttpPut("user/{userId}/addRole/{newRole}")]
    public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
    {
        try
        {
            var result = await adminService.ChangeUserRole(userId, newRole);
            return new OkObjectResult(result);
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
    
    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var result = await adminService.DeleteUser(id);
            return new OkObjectResult(result);
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
    
    // get users 
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var result = await adminService.GetAllUsers();
            return new OkObjectResult(result);
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
    
    
}