using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICardService
{
    Task<Card> AddToCardAsync(CreateCardDTO cardDto, string userToken);
    Task<Card> GetCard(int cardId);
}