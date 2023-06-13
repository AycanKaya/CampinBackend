using System.ComponentModel.DataAnnotations;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CampinWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CampsiteOwnerController : ControllerBase
{
    private readonly ICampsiteOwnerService campsiteOwnerService;
    
    public CampsiteOwnerController( ICampsiteOwnerService campsiteOwnerService)
    {
        this.campsiteOwnerService = campsiteOwnerService;
    }
    
    [HttpPost("CreateCampsite")]
    public async Task<BaseResponseModel<Campsite>> CreateCampsite(CreateCampsiteRequestDTO campsiteRequestDto)
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var createdCampsite = await campsiteOwnerService.CreateCampsite(campsiteRequestDto, userToken);
            var result = new BaseResponseModel<Campsite>(createdCampsite, "Campsite created.");
            return result;
        }
        catch (UnauthorizedAccessException exception)
        {
            throw new UnauthorizedAccessException(exception.Message);
        }
        catch (ValidationException exception)
        {
            throw new  ValidationException(exception.Message);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
    [HttpDelete("DeleteCampsite")]
    public async Task<BaseResponseModel<bool>> DeleteCampsite(string id)
    {
        var isDeleted = await campsiteOwnerService.DeleteCampsite(id);
        var result = new BaseResponseModel<bool>(isDeleted, "Campsite deleted");
        return result;
    }
    
    [HttpPut("UpdateCampsite")]
    public async Task<BaseResponseModel<Campsite>> UpdateCampsite(UpdateCampsiteDTO dto)
    {
        var updatedCampsite = await campsiteOwnerService.UpdateCampsite(dto);
        var result = new BaseResponseModel<Campsite>(updatedCampsite, "Campsite updated");
        return result;
    }

}