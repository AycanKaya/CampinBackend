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
public class FavoriteCampsiteController : ControllerBase
{
    private readonly IFavoriteCampsitesService favoriteCampsitesService;
    
    public FavoriteCampsiteController( IFavoriteCampsitesService favoriteCampsitesService)
    {
        this.favoriteCampsitesService = favoriteCampsitesService;
    }
    
    [HttpPost("AddFavoriteCampsite")]
    public async Task<BaseResponseModel<bool>> AddFavoriteCampsite(string campsiteId)
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var addeFavorites = await favoriteCampsitesService.AddCampsiteToFavorites(userToken, campsiteId);
            var result = new BaseResponseModel<bool>(addeFavorites, "Added favorites." );
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
    [HttpGet("GetFavoriteCampsites")]
    public async Task<BaseResponseModel<List<Campsite>>> GetFavoriteCampsites()
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var favoriteCampsites = await favoriteCampsitesService.GetFavoriteCampsites(userToken);
            var result = new BaseResponseModel<List<Campsite>>(favoriteCampsites, "Get favorite campsites.");
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
   

}