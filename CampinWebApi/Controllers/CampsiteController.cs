using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CampsiteController :ControllerBase
{
    private readonly ICampsiteService campsiteService;
    public CampsiteController( ICampsiteService campsiteService)
    {
        this.campsiteService = campsiteService;
    }

    [HttpGet("GetCampsiteById")]
    public async Task<IActionResult> GetCampsite(string id)
    {
        try
        {
            var campsite = await campsiteService.GetCampsiteById(id);
            var result = new BaseResponseModel<CampsitesResponseModel>(campsite, "Campsite found");
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
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
    [HttpGet("GetCampsiteByName")]
    public async Task<BaseResponseModel<Campsite>> GetCampsiteByName(string name)
    {
        var campsite = await campsiteService.GetCampsiteByName(name);
        var result = new BaseResponseModel<Campsite>(campsite, "Campsite found");
        return result;
    }
    
    [HttpGet("GetCampsites")]
    public async Task<IActionResult> GetCampsites()
    {
        try
        {
            var campsiteList = await campsiteService.GetAllCampsite();
            var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
            return new  OkObjectResult(result);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch(Exception exception)
        { 
            throw new ExceptionResponse(exception.Message);
        }

    }

    [HttpGet("GetAvailableCampsites")]
    public async Task<IActionResult> GetAvailableCampsites(string startDate, string endDate, string city, string holidayDestination)
    {
        try
        {
            var campsiteList = await campsiteService.GetAvailableCampsites(city, holidayDestination, startDate, endDate);
            var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
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
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
    [HttpGet("GetPopulerCampsites")]
    public async Task<IActionResult> GetPopulerCampsites()
    {
        try
        {
            var campsiteList = await campsiteService.GetPopulerCampsites();
            var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
            return new OkObjectResult(result);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
}