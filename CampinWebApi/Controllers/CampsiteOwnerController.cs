using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
[Authorize(Roles = "Owner")]
public class CampsiteOwnerController : ControllerBase
{
    private readonly ICampsiteOwnerService campsiteOwnerService;
    
    public CampsiteOwnerController( ICampsiteOwnerService campsiteOwnerService)
    {
        this.campsiteOwnerService = campsiteOwnerService;
    }
    
    [HttpPost("CreateCampsite")]
    public async Task<IActionResult> CreateCampsite(CreateCampsiteRequestDTO campsiteRequestDto)
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var createdCampsite = await campsiteOwnerService.CreateCampsite(campsiteRequestDto, userToken);
            var result = new BaseResponseModel<Campsite>(createdCampsite, "Campsite created.");
            return new OkObjectResult(result);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Only owners can create campsite!" , (int)HttpStatusCode.Unauthorized);
            return new UnauthorizedObjectResult(response);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    [HttpDelete("DeleteCampsite")]
    public async Task<IActionResult> DeleteCampsite(string id)
    {
        try
        {
            var isDeleted = await campsiteOwnerService.DeleteCampsite(id);
            var result = new BaseResponseModel<bool>(isDeleted, "Campsite deleted");
            return new OkObjectResult(result);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Only owner can delete this campsite!" , (int)HttpStatusCode.Unauthorized);
            return new UnauthorizedObjectResult(response);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
    [HttpPut("UpdateCampsite")]
    public async Task<IActionResult> UpdateCampsite(UpdateCampsiteDTO dto)
    {
        try
        {
            var updatedCampsite = await campsiteOwnerService.UpdateCampsite(dto);
            var result = new BaseResponseModel<Campsite>(updatedCampsite, "Campsite updated");
            return new OkObjectResult(result);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Only owner can update campsite!" , (int)HttpStatusCode.Unauthorized);
            return new UnauthorizedObjectResult(response);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }

}