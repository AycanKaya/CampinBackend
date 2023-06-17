using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.PaymentDTO;
using Microsoft.AspNetCore.Http;

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
            throw new BadHttpRequestException("Invalid credit card number");
        }
        
        if (payment.ExpirationDate != this.creditCardMock.ExpirationDate)
        {
            throw new BadHttpRequestException("Invalid credit card expiration date");
        }
        
        if (payment.SecurityCode != this.creditCardMock.SecurityCode)
        {
            throw new BadHttpRequestException("Invalid credit card security code");
        }
        
        if(payment.CardTotalPrice > this.creditCardMock.Amount)
        {
            throw new BadHttpRequestException("Insufficient funds");
        }

        if (payment.CardHolderName != this.creditCardMock.CardHolderName)
        {
            throw new BadHttpRequestException("Invalid credit card holder name");
        }
        
        creditCardMock.Amount -= payment.CardTotalPrice;
        return true;
    } 
    
}