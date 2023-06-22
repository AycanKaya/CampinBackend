namespace CampinWebApi.Core.Models.CampsiteModels;

public class GetAllCampsitesResponseModel
{
    public string CampsiteId { get; set; }
    public string HolidayDestinationName{ get; set; }
    public string CityName { get; set; }
    public string Name { get; set; }
    public string lng { get; set; }
    public string lat { get; set; }
    public string Description { get; set; }
    public float Rate { get; set; }
    public float AdultPrice { get; set; }
    public float ChildPrice { get; set; }
    public int Capacity { get; set; }
    public string defaultImage { get; set; }
    public int? reviewCount { get; set; }
    
    public DateTime SeasonStartDate { get; set; }
    public DateTime SeasonCloseDate { get; set; }    
}