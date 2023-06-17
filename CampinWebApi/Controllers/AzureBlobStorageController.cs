using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.BlobStorageDTO;
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
    public async Task<BaseResponseModel<BlobStorageResponseModel>> UploadFile(UploadFileRequestDTO file)
    {
        var isUpload = await azureBlobStorageService.UploadFileToBlobStorage(file);
        return new BaseResponseModel<BlobStorageResponseModel>(isUpload);
    }
}