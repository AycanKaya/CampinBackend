namespace CampinWebApi.Services.CrediCardMock;

public class CreditCard
{
    public string CardNumber { get; set; } = "4242424242424242";
    public string ExpirationDate { get; set; } = "10/24";
    public int SecurityCode { get; set; } = 123;
    public string CardHolderName { get; set; } = "John Doe";
    public float Amount { get; set; } = 1000;
}