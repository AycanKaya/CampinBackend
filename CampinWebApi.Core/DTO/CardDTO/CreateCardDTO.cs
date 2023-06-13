using CampinWebApi.Domain.Enums;

namespace CampinWebApi.Core.DTO.CardDTO;

public class CreateCardDTO
{
    public string CampsiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChilder { get; set; }
    
    public float TotalPrice { get; set; }
}