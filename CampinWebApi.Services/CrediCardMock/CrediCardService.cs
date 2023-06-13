using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.PaymentDTO;

namespace CampinWebApi.Services.CrediCardMock;

public class CrediCardService : ICrediCardService
{
    private  CreditCard creditCardMock;
    
    public CrediCardService()
    {
        this.creditCardMock = new CreditCard();
    }
    
    
    public bool ProcessPaymentAsync(PaymentDTO payment)
    {
        if (payment.CardNumber != this.creditCardMock.CardNumber)
        {
            throw new ArgumentException("Invalid credit card number");
        }
        
        if (payment.ExpirationDate != this.creditCardMock.ExpirationDate)
        {
            throw new ArgumentException("Invalid credit card expiration date");
        }
        
        if (payment.SecurityCode != this.creditCardMock.SecurityCode)
        {
            throw new ArgumentException("Invalid credit card security code");
        }
        
        if(payment.CardTotalPrice > this.creditCardMock.Amount)
        {
            throw new ArgumentException("Insufficient funds");
        }
        
        creditCardMock.Amount -= payment.CardTotalPrice;
        return true;
    } 
    
}