namespace CampinWebApi.Core.DTO.RezervationDTO;

public class MakeRezervationDTO
{
    public string CampsiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChilder { get; set; }
    
    public float TotalPrice { get; set; }
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string ExpirationDate { get; set; }
    public int SecurityCode { get; set; }
    public float CardTotalPrice { get; set; }
}