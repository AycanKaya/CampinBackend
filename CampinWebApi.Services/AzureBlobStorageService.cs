using System.Net.Mime;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.BlobStorageDTO;
using CampinWebApi.Core.Models.BlobStorageModel;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
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

    public async Task<BlobStorageResponseModel> UploadFileToBlobStorage(UploadFileRequestDTO fileRequestDto)
    {
        BlobStorageResponseModel response = new();
        BlobContainerClient container = new BlobContainerClient(storageConnectionString, "campsiteimages");
        
        try
        {
            BlobClient client = container.GetBlobClient(fileRequestDto.blob.FileName);

            await using (Stream? data = fileRequestDto.blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"File {fileRequestDto.blob.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.BlobStorageUrl = client.Uri.AbsoluteUri;
            response.Blob.BlobName = client.Name;
            response.Blob.ContentType = fileRequestDto.blob.ContentType;
            response.Blob.Content = fileRequestDto.blob.OpenReadStream();
        }
        // If the file already exists, we catch the exception and do not upload it
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            logger.LogError(
                $"File with name {fileRequestDto.blob.FileName} already exists in container. Set another name to store the file in the container: campsiteimages.");
            response.Status =
                $"File with name {fileRequestDto.blob.FileName} already exists. Please use another name to store your file.";
            response.Error = true;
            return response;
        }
        // If we get an unexpected error, we catch it here and return the error message
        catch (RequestFailedException ex)
        {
            // Log error to console and create a new response we can return to the requesting method
            logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
            response.Error = true;
            return response;
        }
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == fileRequestDto.campsiteId);
        if (campsite == null)
        {
            logger.LogError($"Campsite with id {fileRequestDto.campsiteId} not found.");
            response.Status = $"Campsite with id {fileRequestDto.campsiteId} not found.";
            response.Error = true;
            return response;
        }
        
        var campsiteImage = new CampsiteImages
        {
            CampsiteId = campsite.CampsiteId,
            ImageUrl = response.Blob.BlobStorageUrl
        };
        
        await context.CampsiteImages.AddAsync(campsiteImage);
        context.SaveChanges();
        return response;

    }
}