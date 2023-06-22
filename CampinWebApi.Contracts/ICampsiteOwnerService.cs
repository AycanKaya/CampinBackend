using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Core.Models.CampsiteOwnerModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICampsiteOwnerService
{
    Task<CampsiteModel> CreateCampsite(CreateCampsiteRequestDTO request, string userToken);
    Task<CampsiteModel> UpdateCampsite(UpdateCampsiteDTO dto);
    Task<bool> DeleteCampsite(string id);
    Task<CampsiteResponseModel[]> GetOwnerCampsites(string usertoken);
}