using System.ComponentModel.DataAnnotations;
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
    public async Task<BaseResponseModel<CampsiteResponseModel>> GetCampsite(string id)
    {
        var campsite = await campsiteService.GetCampsiteById(id);
        var result = new BaseResponseModel<CampsiteResponseModel>(campsite, "Campsite found");
        return result;
    }
    
    [HttpGet("GetCampsiteByName")]
    public async Task<BaseResponseModel<Campsite>> GetCampsiteByName(string name)
    {
        var campsite = await campsiteService.GetCampsiteByName(name);
        var result = new BaseResponseModel<Campsite>(campsite, "Campsite found");
        return result;
    }
    
    [HttpGet("GetCampsites")]
    public async Task<BaseResponseModel<List<Campsite>>> GetCampsites()
    {
        var campsiteList = await campsiteService.GetAllCampsite();
        var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
        return result;
    }

    [HttpGet("GetAvailableCampsites")]
    public async Task<BaseResponseModel<List<Campsite>>> GetAvailableCampsites(DateTime startDate, DateTime endDate, string city, string holidayDestination)
    {
        var campsiteList = await campsiteService.GetAvailableCampsites(city, holidayDestination, startDate, endDate);
        var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
        return result;
    }
    
    [HttpPost("RatingCampsite")]
    public async Task<BaseResponseModel<double>> RatingCampsite(RatingCampsiteDTO ratingDto)
    {
        var userToken = HttpContext.Request.Headers.Authorization.ToString();
        var rating = await campsiteService.RatingCampsite(ratingDto, userToken);
        var result = new BaseResponseModel<double>(rating, "Campsite rated");
        return result;
    }
    
    [HttpGet("GetPopulerCampsites")]
    public async Task<BaseResponseModel<List<Campsite>>> GetPopulerCampsites()
    {
        var campsiteList = await campsiteService.GetPopulerCampsites();
        var result = new BaseResponseModel<List<Campsite>>(campsiteList, "Campsites found");
        return result;
    }
    
}