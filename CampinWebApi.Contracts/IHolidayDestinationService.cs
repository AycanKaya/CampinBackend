using CampinWebApi.Core.DTO.HolidayDestinationDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface IHolidayDestinationService
{
    Task<HolidayDestination> AddHolidayDestination(AddHolidayDestinationDTO dto);
    Task<HolidayDestination> UpdateHolidayDestination(UpdateHolidayDestinationDTO dto);
    Task<HolidayDestination> DeleteHolidayDestination(int id);
    Task<HolidayDestination> GetHolidayDestinationById(int id);
    Task<IEnumerable<HolidayDestination>> GetAllHolidayDestinations();
    
}