using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CampsiteService :ICampsiteService
{
    private readonly CampinDbContext context;
    private readonly IJWTService jwtService;

    public CampsiteService(CampinDbContext context, IJWTService jwtService)
    {
        this.context = context;
        this.jwtService = jwtService;
    }
    
    /*
    public async Task<List<Campsite>> GetAvailableCampsites(DateOnly startDate, DateOnly endDate, string city, string country)
    {
        var unavailableCampsiteIds = context.Rezervations
            .Where(r => r.StartDate <= endDate && r.EndDate >= startDate)
            .Select(r => r.CampsiteId)
            .Distinct()
            .ToList();

        var availableCampsites = await context.Campsites
            .Join(context.VacationSpots,
                c => c.VacationSpotID,
                v => v.Id,
                (c, v) => new { Campsite = c, VacationSpot = v })
            .Where(x => !unavailableCampsiteIds.Contains(x.Campsite.CampsiteId) && x.VacationSpot.City == city && x.VacationSpot.Country == country)
            .Select(x => x.Campsite)
            .ToListAsync();

        return availableCampsites;
    }  */ 
    
    public async Task<List<Campsite>> GetAvailableCampsites(string cityName, string holidayDestinationName, DateTime startDate, DateTime endDate)
    {
        int holidayDestinationId = context.HolidayDestinations
            .Join(context.Cities , hd => hd.CityId , c => c.Id , (hd , c) => new {HolidayDestination = hd , City = c})
            .Where(hd => hd.HolidayDestination.HolidayDestinationName == holidayDestinationName && hd.City.CityName == cityName)
            .Select(hd => hd.HolidayDestination.Id)
            .FirstOrDefault();

        if (holidayDestinationId == 0)
            return new List<Campsite>();

        var reservedCampsites = context.Rezervations
            .Where(r => r.StartDate <= endDate && r.EndDate >= startDate)
            .Select(r => r.CampsiteId)
            .ToList();
        
        var availableCampsites = await context.Campsites
            .Where(c => c.HolidayDestinationId == holidayDestinationId && !reservedCampsites.Contains(c.CampsiteId))
            .ToListAsync();

        return availableCampsites;
    }

    public async Task<Campsite> CreateCampsite(CreateCampsiteRequestDTO request, string userToken)
    {
        var ownerID =  jwtService.GetUserIdFromJWT(userToken);
        var feature = new Features
        {
            HasElectricity = request.HasElectricity,
            HasWater = request.HasWater,
            HasToilet = request.HasToilet,
            HasShower = request.HasShower,
            HasWiFi = request.HasWiFi,
            HasTrees = request.HasTrees,
            HasParking = request.HasParking,
            HasSecurity = request.HasSecurity,
            HasBusinessServices = request.HasBusinessServices,
            HasActivities = request.HasActivities,
            HasFirePit = request.HasFirePit,
            HasSignal = request.HasSignal,
            IsNearSea = request.IsNearSea
        };
        
        context.Features.Add(feature);
        
        var campsite = new Campsite
        {
            CampsiteId = Guid.NewGuid().ToString(),
            Name = request.Name,
            OwnerID = ownerID,
            HolidayDestinationId = request.HolidayDestinationId,
            Description = request.Description,
            AdultPrice = request.AdultPrice,
            ChildPrice = request.ChildPrice,
            Capacity = request.Capacity,
            SeasonStartDate = request.SeasonStartDate,
            SeasonCloseDate = request.SeasonCloseDate,
            FeatureId = feature.Id,
            isEnable = true
        };
        context.Campsites.Add(campsite);
        await context.SaveChangesAsync();
        return campsite;
    }
    
    
    public async Task<double> RatingCampsite(RatingCampsiteDTO ratingDto,string userToken)
    {
        var userID =  jwtService.GetUserIdFromJWT(userToken);

        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == ratingDto.CampsiteId);
        if (campsite == null)
            throw new Exception($"Campsite with id {ratingDto.CampsiteId} not found");
        
        var Rating = new Rating
        {
            RatingId = Guid.NewGuid().ToString(),
            CampsiteId = ratingDto.CampsiteId,
            UserId = userID,
            Rate = ratingDto.Rating
        };        
        var ratingsForThis = await context.Ratings.Where(r => r.CampsiteId == ratingDto.CampsiteId).ToListAsync();
        var sum = 0.0;
        foreach (var rating in ratingsForThis)
        {
            sum += rating.Rate;
        }
        campsite.Rate = (float)(sum / ratingsForThis.Count);
        context.Ratings.Add(Rating);
        context.SaveChangesAsync();
        return campsite.Rate;
    }
    
    // Bu kamp alanına ait rezerve edilmiş  tüm tarihleri döndürür.
    public async Task<CampsiteResponseModel> GetCampsiteById(string id)
    {
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == id);
        
        if (campsite == null)
            throw new Exception($"Campsite with id {id} not found");

        var commentList= await context.Comments.Where(x => x.CampsiteId == id).ToArrayAsync();
        var imageUrls = await context.CampsiteImages.Where(x => x.CampsiteId == id).Select(x => x.ImageUrl).ToArrayAsync();
        
        var campsiteResponse = new CampsiteResponseModel
        {
            Campsite = campsite,
            Comments = commentList,
            ImageUrls = imageUrls
        };
        return campsiteResponse;
    }
    public async Task<Campsite> GetCampsiteByName(string name)
    {
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.Name == name);
        
        if (campsite == null)
            throw new Exception($"Campsite with id {name} not found");
        
        return campsite;
    }
  
    public async Task<List<Campsite>> GetAllCampsite()
    {
        return await context.Campsites.ToListAsync();
    } 
    
    public async Task<List<Campsite>> GetPopulerCampsites() 
    {
      var campsites = await context.Campsites.ToListAsync();

      return campsites
          .OrderByDescending(c => c.Rate)
          .Take(10)
          .ToList(); 
    }
}