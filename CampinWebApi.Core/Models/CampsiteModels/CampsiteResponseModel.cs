using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Core.Models.CampsiteModels;

public class CampsiteResponseModel
{
    public Campsite Campsite { get; set; }
    public Comments[] Comments { get; set; }
    public string[] ImageUrls { get; set; }
}