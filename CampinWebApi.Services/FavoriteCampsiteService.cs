using CampinWebApi.Contracts;
using CampinWebApi.Core.Models.CampsiteModels;
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

    public async Task<List<CampsiteResponseModel>> GetFavoriteCampsites(string token)
    {
        var userId = jwtService.GetUserIdFromJWT(token);
        var owner = await dbContext.UserInfo.FirstOrDefaultAsync(x => x.UserID == userId);


        var favoriteCampsiteIds = dbContext.FavoriteCampsites
            .Where(f => f.userId == userId)
            .Select(f => f.campsiteId)
            .ToList();

        var favoriteCampsites = await dbContext.Campsites
            .Include(x => x.HolidayDestination)
            .Include(x => x.HolidayDestination.City)
            .Where(c => favoriteCampsiteIds.Contains(c.CampsiteId))
            .Select(campsite => new CampsiteResponseModel
            {
                CampsiteId = campsite.CampsiteId,
                Name = campsite.Name,
                HolidayDestinationName =  campsite.HolidayDestination.HolidayDestinationName,
                CityName = campsite.HolidayDestination.City.CityName,
                OwnerId = owner.UserID,
                Description = campsite.Description,
                AdultPrice = campsite.AdultPrice,
                ChildPrice = campsite.ChildPrice,
                Capacity = campsite.Capacity,
                SeasonStartDate = campsite.SeasonStartDate,
                SeasonCloseDate = campsite.SeasonCloseDate,
                OwnerEmail = owner.Email,
                OwnerName = owner.Name,
                OwnerSurname = owner.Surname,
                OwnerPhoneNumber = owner.PhoneNumber,
                Rate = campsite.Rate,
                lng = campsite.lng,
                lat = campsite.lat,
            })
            .ToListAsync();

        return favoriteCampsites;
    }

    public async Task<bool> RemoveFavoriteCampsite(string campsiteId, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var favoriteCampsites =
            await dbContext.FavoriteCampsites.FirstOrDefaultAsync(x =>
                x.campsiteId == campsiteId && x.userId == userId);
        if (favoriteCampsites == null)
            throw new FileNotFoundException("This favorite campsite did not found");
        
        dbContext.FavoriteCampsites.Remove(favoriteCampsites);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
