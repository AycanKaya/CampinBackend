using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CardService : ICardService
{
    private readonly CampinDbContext context;
    public CardService(CampinDbContext context)
    {
        this.context = context;
    }
    
    public async Task<Card> AddToCardAsync(CreateCardDTO cardDto, string userInfoId)
    {
        var card = new Card()
        { 
            CampsiteId = cardDto.CampsiteId,
            UserInfoID = userInfoId,
            StartDate = cardDto.StartDate,
            EndDate = cardDto.EndDate,
            NumOfAdult = cardDto.NumOfAdult,
            NumOfChilder = cardDto.NumOfChilder,
            TotalPrice = cardDto.TotalPrice,
            isPaid = false,
            isEnable = true,
        };
        this.context.Cards.Add(card);
        await this.context.SaveChangesAsync();
        return card;
    }

    public async Task<Card> GetCard(int id)
    {
        var card = await this.context.Cards.FirstOrDefaultAsync(c => c.Id == id);
        return card;
    }
    
}