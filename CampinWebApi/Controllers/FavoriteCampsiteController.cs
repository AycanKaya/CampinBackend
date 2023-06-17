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
public class FavoriteCampsiteController : ControllerBase
{
    private readonly IFavoriteCampsitesService favoriteCampsitesService;
    
    public FavoriteCampsiteController( IFavoriteCampsitesService favoriteCampsitesService)
    {
        this.favoriteCampsitesService = favoriteCampsitesService;
    }
    
    [HttpPost("AddFavoriteCampsite")]
    public async Task<IActionResult> AddFavoriteCampsite(string campsiteId)
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var addeFavorites = await favoriteCampsitesService.AddCampsiteToFavorites(userToken, campsiteId);
            var result = new BaseResponseModel<bool>(addeFavorites, "Added favorites." );
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
    
    [HttpGet("GetFavoriteCampsites")]
    public async Task<IActionResult> GetFavoriteCampsites()
    {
        try
        {
            var userToken = HttpContext.Request.Headers.Authorization.ToString();
            var favoriteCampsites = await favoriteCampsitesService.GetFavoriteCampsites(userToken);
            var result = new BaseResponseModel<List<Campsite>>(favoriteCampsites, "Get favorite campsites.");
            return new OkObjectResult(result);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception exception)
        {
            return new InternalServerErrorResult();
        }
    }
   

}