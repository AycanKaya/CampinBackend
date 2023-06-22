using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.Models;
using CampinWebApi.Core.Models.BlobStorageModel;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AzureBlobStorageController : ControllerBase
{
    private IAzureBlobStorageService azureBlobStorageService;
    
    public AzureBlobStorageController(IAzureBlobStorageService azureBlobStorageService)
    {
        this.azureBlobStorageService = azureBlobStorageService;
    }
    
    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile[] file)
    {
        try
        {
            var isUpload = await azureBlobStorageService.UploadFilesToBlobStorage(file);
            var result = new BaseResponseModel<string[]>(isUpload, "Campsite found");
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