using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface IFavoriteCampsitesService
{
    Task<bool> AddCampsiteToFavorites(string userId, string campsiteId);
    Task<List<CampsiteResponseModel>> GetFavoriteCampsites(string userId);
    Task<bool> RemoveFavoriteCampsite(string campsiteId, string userToken);
}