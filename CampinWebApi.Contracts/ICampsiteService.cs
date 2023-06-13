using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICampsiteService
{
    Task<CampsiteResponseModel> GetCampsiteById(string id);
    Task<Campsite> GetCampsiteByName(string name);

    Task<List<Campsite>> GetAvailableCampsites(string cityName, string holidayDestinationName, DateTime startDate,
        DateTime endDate);
    Task<List<Campsite>> GetAllCampsite();
    Task<double> RatingCampsite(RatingCampsiteDTO ratingDto, string userToken);
    Task<List<Campsite>> GetPopulerCampsites();
}