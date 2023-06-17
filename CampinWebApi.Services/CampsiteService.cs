using System.Globalization;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Core.Models.CommentModels;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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
    public async Task<List<GetAllCampsitesResponseModel>> GetAvailableCampsites(string cityName, string holidayDestinationName, string start, string end)
    {
        DateTime startDate = DateTime.TryParseExact(start, "yyyy/MM/dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date) ? date : throw new BadHttpRequestException("Invalid date format please use yyyy/MM/dd");
        
        DateTime endDate = DateTime.TryParseExact(end, "yyyy/MM/dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var enddate) ? enddate : throw new BadHttpRequestException("Invalid date format please use yyyy/MM/dd");
        
        int holidayDestinationId = await context.HolidayDestinations
            .Join(context.Cities , hd => hd.CityId , c => c.Id , (hd , c) => new {HolidayDestination = hd , City = c})
            .Where(hd => hd.HolidayDestination.HolidayDestinationName == holidayDestinationName && hd.City.CityName == cityName)
            .Select(hd => hd.HolidayDestination.Id)
            .FirstOrDefaultAsync();

        var reservedCampsites = await context.Rezervations
            .Where(r => r.StartDate <= endDate && r.EndDate >= startDate)
            .Select(r => r.CampsiteId)
            .ToListAsync();
        
        var availableCampsites = await context.Campsites
            .Where(c => c.SeasonStartDate <= startDate && c.SeasonCloseDate >= endDate &&  c.HolidayDestinationId == holidayDestinationId && !reservedCampsites.Contains(c.CampsiteId))
            .Include(c => c.HolidayDestination)
            .ThenInclude(hd => hd.City)
            .ToListAsync();
        
        var campsiteResponseList = availableCampsites.Select(c => new GetAllCampsitesResponseModel
        {
            CampsiteId = c.CampsiteId,
            HolidayDestinationName = c.HolidayDestination.HolidayDestinationName,
            CityName = c.HolidayDestination.City.CityName,
            Name = c.Name,
            Description = c.Description,
            Rate = c.Rate,
            AdultPrice = c.AdultPrice,
            ChildPrice = c.ChildPrice,
            Capacity = c.Capacity
        }).ToList();
        
        return campsiteResponseList;
    }
    public async Task<double> RatingCampsite(RatingCampsiteDTO ratingDto,string userToken)
    {
        var userID =  jwtService.GetUserIdFromJWT(userToken);

        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == ratingDto.CampsiteId);
        if (campsite == null)
            throw new FileNotFoundException($"Campsite with id {ratingDto.CampsiteId} not found");
        
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
    public async Task<CampsitesResponseModel> GetCampsiteById(string id)
    {
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == id);
        var owner = await context.UserInfo.FirstOrDefaultAsync(x => x.UserID == campsite.OwnerID);
        
        var holidayDestination = await context.HolidayDestinations.FirstOrDefaultAsync(x => x.Id == campsite.HolidayDestinationId);
        var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == holidayDestination.CityId);
        
        var campsiteModel = new CampsiteResponseModel
        {
            CampsiteId = campsite.CampsiteId,
            Name = campsite.Name,
            HolidayDestinationName =  holidayDestination.HolidayDestinationName,
            CityName = city.CityName,
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
        };

        if (campsite == null)
            throw new FileNotFoundException($"Campsite with id {id} not found");

        var commentList = await context.Comments.
            Include(x => x.Author)
            .Where(x => x.CampsiteId == id).ToArrayAsync();

        var commentResponseModelList = new List<GetCommentModel>();
        foreach (var comment in commentList)
        {
            var rate = await context.Ratings.FirstOrDefaultAsync(c => c.CampsiteId == id && c.UserId == comment.AuthorId);
            var response = new GetCommentModel
            {
                Content = comment.Content,
                AuthorName = comment.AuthorName,
                AuthorSurname = comment.Author.Surname,
                UserRate = rate.Rate,
                Created = comment.Created.Date
            };
            commentResponseModelList.Add(response);
        }
        var featureList = await context.Features.Where(x => x.Id == campsite.FeatureId).FirstOrDefaultAsync();
        var imageUrls = await context.CampsiteImages.Where(x => x.CampsiteId == id).Select(x => x.ImageUrl).ToArrayAsync();
        
        var campsiteResponse = new CampsitesResponseModel
        {
            Campsite = campsiteModel,
            Comments = commentResponseModelList.ToArray(),
            ImageUrls = imageUrls,
            Features = featureList
        };
        return campsiteResponse;
    }

    public async Task<List<GetAllCampsitesResponseModel>> GetAllCampsites()
    {
        var campsites = await context.Campsites
            .Include(c => c.HolidayDestination)
            .ThenInclude(hd => hd.City)
            .ToListAsync();

        var campsiteResponseList = campsites.Select(c => new GetAllCampsitesResponseModel
        {
            CampsiteId = c.CampsiteId,
            HolidayDestinationName = c.HolidayDestination.HolidayDestinationName,
            CityName = c.HolidayDestination.City.CityName,
            Name = c.Name,
            Description = c.Description,
            Rate = c.Rate,
            AdultPrice = c.AdultPrice,
            ChildPrice = c.ChildPrice,
            Capacity = c.Capacity
        }).ToList();

        return campsiteResponseList;
    }

    public async Task<List<GetAllCampsitesResponseModel>> GetPopulerCampsites() 
    {
      var campsites = await context.Campsites
          .Include(c => c.HolidayDestination)
          .ThenInclude(hd => hd.City)
          .ToListAsync();

      var campsiteResponseList = campsites.Select(c => new GetAllCampsitesResponseModel
      {
          CampsiteId = c.CampsiteId,
          HolidayDestinationName = c.HolidayDestination.HolidayDestinationName,
          CityName = c.HolidayDestination.City.CityName,
          Name = c.Name,
          Description = c.Description,
          Rate = c.Rate,
          AdultPrice = c.AdultPrice,
          ChildPrice = c.ChildPrice,
          Capacity = c.Capacity
      }).ToList();

      return campsiteResponseList
          .OrderByDescending(c => c.Rate)
          .Take(10)
          .ToList(); 
    }
     
    // method that returns all campsites according to city
    public async Task<List<GetAllCampsitesResponseModel>> GetCampsitesByCity(string city)
    {
        var campsites = await context.Campsites
            .Include(c => c.HolidayDestination)
            .ThenInclude(hd => hd.City)
            .ToListAsync();
        
        var filteredCampsites = campsites
            .Where(c => string.Equals(c.HolidayDestination.City.CityName, city, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var campsiteResponseList = filteredCampsites.Select(c => new GetAllCampsitesResponseModel
        {
            CampsiteId = c.CampsiteId,
            HolidayDestinationName = c.HolidayDestination.HolidayDestinationName,
            CityName = c.HolidayDestination.City.CityName,
            Name = c.Name,
            Description = c.Description,
            Rate = c.Rate,
            AdultPrice = c.AdultPrice,
            ChildPrice = c.ChildPrice,
            Capacity = c.Capacity
        }).ToList();
        
        return campsiteResponseList;
    }
    
}