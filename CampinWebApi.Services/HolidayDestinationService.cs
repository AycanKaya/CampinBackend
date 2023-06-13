using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.HolidayDestinationDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class HolidayDestinationService :IHolidayDestinationService
{
    private readonly CampinDbContext context;
    public HolidayDestinationService(CampinDbContext context)
    {
        this.context = context;
    }
    
    // add holiday destination
    public async Task<HolidayDestination> AddHolidayDestination(AddHolidayDestinationDTO dto)
    {
        var holidayDestination = new HolidayDestination
        {
            CityId = dto.CityId,
            HolidayDestinationName = dto.HolidayDestinationName,
            Information = dto.Information,
        };
        await context.HolidayDestinations.AddAsync(holidayDestination);
        await context.SaveChangesAsync();
        return holidayDestination;
    }
    // update holiday destination
    public async Task<HolidayDestination> UpdateHolidayDestination(UpdateHolidayDestinationDTO dto)
    {
        var holidayDest= await context.HolidayDestinations.FirstOrDefaultAsync(x => x.Id == dto.Id);
        
        holidayDest.CityId = dto.CityId;
        holidayDest.HolidayDestinationName = dto.HolidayDestinationName;
        holidayDest.Information = dto.Information;
        
        context.HolidayDestinations.Update(holidayDest);
        await context.SaveChangesAsync();
        return holidayDest;
    }
    // delete holiday destination
    public async Task<HolidayDestination> DeleteHolidayDestination(int id)
    {
        var holidayDestination = await context.HolidayDestinations.FindAsync(id);
        context.HolidayDestinations.Remove(holidayDestination);
        await context.SaveChangesAsync();
        return holidayDestination;
    }
    // get holiday destination by id
    public async Task<HolidayDestination> GetHolidayDestinationById(int id)
    {
        return await context.HolidayDestinations.FindAsync(id);
    }
    // get all holiday destinations
    public async Task<IEnumerable<HolidayDestination>> GetAllHolidayDestinations()
    {
        return await context.HolidayDestinations.ToListAsync();
    }
}