namespace CampinWebApi.Core.DTO.PaymentDTO;

public class PaymentDTO
{
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public int SecurityCode { get; set; }
    public string CardHolderName { get; set; }
    public float CardTotalPrice { get; set; }
}