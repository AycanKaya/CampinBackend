namespace CampinWebApi.Core.DTO.CampsiteDTO;

public class CreateCampsiteRequestDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AdultPrice { get; set; }
    public int ChildPrice { get; set; }
    public int HolidayDestinationId { get; set; }
    public DateTime SeasonStartDate { get; set; }
    public DateTime SeasonCloseDate { get; set; }
    public string lat { get; set; }
    public string lng { get; set; }
    public int Capacity { get; set; }
    public bool HasElectricity { get; set; }
    public bool HasWater { get; set; }
    public bool HasToilet { get; set; }
    public bool HasShower { get; set; }
    public bool HasWiFi { get; set; }
    public bool HasTrees { get; set; }
    public bool HasParking { get; set; }
    public bool HasSecurity { get; set; }
    public bool HasBusinessServices { get; set; }
    public bool HasActivities { get; set; }
    public bool HasFirePit { get; set; }
    public bool HasSignal { get; set; }
    public bool IsNearSea { get; set; }
    public string[] ImageUrls { get; set; }
}