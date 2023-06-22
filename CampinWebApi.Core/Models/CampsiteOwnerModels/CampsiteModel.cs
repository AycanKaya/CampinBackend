namespace CampinWebApi.Core.Models.CampsiteOwnerModels;

public class CampsiteModel
{
    public string CampsiteId { get; set; }
    public int HolidayDestinationId { get; set; }
    public int FeatureId { get; set; }
    public string OwnerID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Rate { get; set; }
    public float AdultPrice { get; set; }
    public float ChildPrice { get; set; }
    public string lat { get; set; }
    public string lng { get; set; }
    public int Capacity { get; set; }
    public string DefaultImage { get; set; }
}