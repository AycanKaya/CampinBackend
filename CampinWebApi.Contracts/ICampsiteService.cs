using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICampsiteService
{
    Task<CampsitesResponseModel> GetCampsiteById(string id);
    Task<List<GetAllCampsitesResponseModel>> GetCampsitesByCity(string city);
    Task<List<GetAllCampsitesResponseModel>> GetAllCampsites();
    Task<List<GetAllCampsitesResponseModel>> GetPopulerCampsites();
    Task<List<GetAllCampsitesResponseModel>> GetAvailableCampsites(string cityName, string holidayDestinationName, string start, string end);
}