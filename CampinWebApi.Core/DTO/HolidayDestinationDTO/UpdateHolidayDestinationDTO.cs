namespace CampinWebApi.Core.DTO.HolidayDestinationDTO;

public class UpdateHolidayDestinationDTO
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string HolidayDestinationName { get; set; }
    public string? Information { get; set; }
}