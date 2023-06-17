using CampinWebApi.Core.DTO.BlobStorageDTO;
using CampinWebApi.Core.Models.BlobStorageModel;

namespace CampinWebApi.Contracts;

public interface IAzureBlobStorageService
{
    Task<BlobStorageResponseModel> UploadFileToBlobStorage(UploadFileRequestDTO fileRequestDto);
    
}