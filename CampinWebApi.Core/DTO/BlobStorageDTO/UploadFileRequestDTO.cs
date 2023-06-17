using Microsoft.AspNetCore.Http;

namespace CampinWebApi.Core.DTO.BlobStorageDTO;

public class UploadFileRequestDTO
{
    public IFormFile blob { get; set; }
    public string campsiteId { get; set; }
}