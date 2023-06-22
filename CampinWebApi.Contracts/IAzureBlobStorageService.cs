using CampinWebApi.Core.DTO.BlobStorageDTO;
using CampinWebApi.Core.Models.BlobStorageModel;

namespace CampinWebApi.Contracts;

public interface IAzureBlobStorageService
{
    Task<string[]> UploadFilesToBlobStorage(IFormFile[] fileRequestDto);

}