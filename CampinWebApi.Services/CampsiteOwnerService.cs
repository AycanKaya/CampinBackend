using System.Globalization;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CampsiteDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CampsiteOwnerService : ICampsiteOwnerService
{
    private readonly CampinDbContext context;
    private readonly IJWTService jwtService;
    
    public CampsiteOwnerService(CampinDbContext context, IJWTService jwtService)
    {
        this.context = context;
        this.jwtService = jwtService;
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
        await context.SaveChangesAsync();

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
            isEnable = true,
            lat = request.lat,
            lng = request.lng,
            Rate = 0
        };
        context.Campsites.Add(campsite);
        await context.SaveChangesAsync();
        return campsite;
    }
    
    public async Task<Campsite> UpdateCampsite(UpdateCampsiteDTO dto)
    {
        DateTime startDate = DateTime.TryParseExact(dto.SeasonStartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date) ? date : throw new BadHttpRequestException("Invalid date format please use yyyy/MM/dd");
        
        DateTime endDate = DateTime.TryParseExact(dto.SeasonCloseDate, "yyyy/MM/dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var end) ? end : throw new BadHttpRequestException("Invalid date format please use yyyy/MM/dd");
        
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == dto.CampsiteId);
        if(campsite == null)
            throw new Exception($"Campsite with id {dto.CampsiteId} not found");

        var features = await context.Features.FirstOrDefaultAsync(f => f.Id == campsite.FeatureId);
        if(features == null)
            throw new Exception($"Features with id {campsite.FeatureId} not found");

        campsite.Name = dto.Name;
        campsite.Description = dto.Description;
        campsite.AdultPrice = dto.AdultPrice;
        campsite.ChildPrice = dto.ChildPrice;
        campsite.HolidayDestinationId = dto.HolidayDestinationId;
        campsite.SeasonStartDate = startDate;
        campsite.SeasonCloseDate = endDate;
        campsite.Capacity = dto.Capacity;
        campsite.lat = dto.lat;
        campsite.lng = dto.lng;
        
        features.HasElectricity = dto.HasElectricity;
        features.HasWater = dto.HasWater;
        features.HasToilet = dto.HasToilet;
        features.HasShower = dto.HasShower;
        features.HasWiFi = dto.HasWiFi;
        features.HasTrees = dto.HasTrees;
        features.HasParking = dto.HasParking;
        features.HasSecurity = dto.HasSecurity;
        features.HasBusinessServices = dto.HasBusinessServices;
        features.HasActivities = dto.HasActivities;
        features.HasFirePit = dto.HasFirePit;
        features.HasSignal = dto.HasSignal;
        features.IsNearSea = dto.IsNearSea;

        await context.SaveChangesAsync();
        return campsite;
    }

    public async Task<bool> DeleteCampsite(string id)
    {
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == id);
        
        if (campsite == null)
            throw new Exception($"Campsite with id {id} not found");
        
        context.Campsites.Remove(campsite);
        await context.SaveChangesAsync();
        return true;
    }
}