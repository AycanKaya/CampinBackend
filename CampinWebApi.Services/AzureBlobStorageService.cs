using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CampinWebApi.Contracts;
using CampinWebApi.Core.Models.BlobStorageModel;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CampinWebApi.Services;
public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly string storageConnectionString;
    private CampinDbContext context;
    private readonly ILogger<AzureBlobStorageService> logger;

    public AzureBlobStorageService(IConfiguration configuration, 
        CampinDbContext context,
        ILogger<AzureBlobStorageService> logger)
    {
        storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
        this.context = context;
        this.logger = logger;
    }


    public async Task<string[]> UploadFilesToBlobStorage(IFormFile[] fileRequestDto)
    {
        var response = new List<string>();
        foreach (var file in fileRequestDto)
        {
            var upload = await UploadFileToBlobStorage(file);
            response.Add(upload);
        }

        return response.ToArray();
    }
    private async Task<string> UploadFileToBlobStorage(IFormFile fileRequestDto)
    {
        BlobStorageResponseModel response = new();
        try
        {
            BlobContainerClient container = new BlobContainerClient(storageConnectionString, "campsiteimages");

            var fileName = Guid.NewGuid().ToString();
            
            BlobClient client = container.GetBlobClient(fileName);
            
            var blobUploadOptions = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders()
                {
                    ContentType = fileRequestDto.ContentType
                }
            };

            await using (Stream? data = fileRequestDto.OpenReadStream())
            {
                await client.UploadAsync(data, blobUploadOptions);
            }
            return client.Uri.AbsoluteUri;
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            logger.LogError(
                $"File with name {fileRequestDto.FileName} already exists in container. Set another name to store the file in the container: campsiteimages.");
            response.Status =
                $"File with name {fileRequestDto.FileName} already exists. Please use another name to store your file.";
            response.Error = true;
            throw ex;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
            response.Error = true;
            throw ex;
        }
    }

}