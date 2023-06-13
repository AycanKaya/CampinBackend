using CampinWebApi.Core.DTO.PaymentDTO;

namespace CampinWebApi.Contracts;

public interface ICrediCardService
{
    bool ProcessPaymentAsync(PaymentDTO payment);
}