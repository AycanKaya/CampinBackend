namespace CampinWebApi.Core.DTO.BlobStorageDTO;

public class BlobDTO
{
    public string? BlobStorageUrl { get; set; }
    public string? BlobName { get; set; }
    public string? ContentType { get; set; }
    public Stream? Content { get; set; }
}