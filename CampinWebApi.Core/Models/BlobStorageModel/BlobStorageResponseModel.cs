using CampinWebApi.Core.DTO.BlobStorageDTO;

namespace CampinWebApi.Core.Models.BlobStorageModel;

public class BlobStorageResponseModel
{ 
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDTO Blob { get; set; }

        public BlobStorageResponseModel()
        {
            Blob = new BlobDTO();
        }
        
}