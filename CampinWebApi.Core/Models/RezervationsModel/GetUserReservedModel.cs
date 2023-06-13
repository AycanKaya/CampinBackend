using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Core.Models.RezervationsModel;

public class GetUserReservedModel
{
    public Campsite Campsite { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChilder { get; set; }
    public bool isPaid { get; set; }
}