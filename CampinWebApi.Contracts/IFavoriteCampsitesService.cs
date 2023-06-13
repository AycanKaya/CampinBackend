using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface IFavoriteCampsitesService
{
    Task<bool> AddCampsiteToFavorites(string userId, string campsiteId);
    Task<List<Campsite>> GetFavoriteCampsites(string userId);
}