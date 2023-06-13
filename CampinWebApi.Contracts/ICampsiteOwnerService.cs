using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICampsiteOwnerService
{
    Task<Campsite> CreateCampsite(CreateCampsiteRequestDTO request, string userToken);
    Task<Campsite> UpdateCampsite(UpdateCampsiteDTO dto);
    Task<bool> DeleteCampsite(string id);
}