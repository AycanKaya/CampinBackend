using CampinWebApi.Contracts;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class FavoriteCampsitesService :IFavoriteCampsitesService
{
    private readonly CampinDbContext dbContext;
    private readonly IJWTService jwtService;

    public FavoriteCampsitesService(CampinDbContext dbContext, IJWTService jwtService)
    {
        this.dbContext = dbContext;
        this.jwtService = jwtService;
    }

    public async Task<bool> AddCampsiteToFavorites(string userToken, string campsiteId)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var favoriteCampsite = new FavoriteCampsites
        {
            userId = userId,
            campsiteId = campsiteId
        };

        dbContext.FavoriteCampsites.Add(favoriteCampsite);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Campsite>> GetFavoriteCampsites(string token)
    {
        var userId = jwtService.GetUserIdFromJWT(token);

        var favoriteCampsiteIds = dbContext.FavoriteCampsites
            .Where(f => f.userId == userId)
            .Select(f => f.campsiteId)
            .ToList();

        var favoriteCampsites = await dbContext.Campsites
            .Where(c => favoriteCampsiteIds.Contains(c.CampsiteId))
            .ToListAsync();

        return favoriteCampsites;
    }
}
