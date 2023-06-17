using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Core.DTO.PaymentDTO;
using CampinWebApi.Core.DTO.RezervationDTO;
using CampinWebApi.Core.Models.RezervationsModel;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using CampinWebApi.Services.CrediCardMock;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class RezervationService : IRezervationService
{
    private ICrediCardService crediCardService;
    private readonly ICardService cardService;
    private readonly IJWTService jwtService;
    private readonly CampinDbContext context;
    
    public RezervationService(ICrediCardService crediCardService, ICardService cardService, IJWTService jwtService, CampinDbContext context)
    {
        this.crediCardService = crediCardService;
        this.cardService = cardService;
        this.jwtService = jwtService;
        this.context = context;
    }

    public async Task<bool> MakeRezervations(MakeRezervationDTO makeRezervationDto, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var cardDto = new CreateCardDTO
        {   
            CampsiteId = makeRezervationDto.CampsiteId,
            StartDate = makeRezervationDto.StartDate,
            EndDate = makeRezervationDto.EndDate,
            NumOfAdult = makeRezervationDto.NumOfAdult,
            NumOfChilder = makeRezervationDto.NumOfChilder,
            TotalPrice = makeRezervationDto.TotalPrice,
        };
        var card = await cardService.AddToCardAsync(cardDto, userId);
        
        var payment = new PaymentDTO
        {
            CardNumber = makeRezervationDto.CardNumber,
            ExpirationDate = makeRezervationDto.ExpirationDate,
            SecurityCode = makeRezervationDto.SecurityCode,
            CardTotalPrice = makeRezervationDto.CardTotalPrice,
            CardHolderName = makeRezervationDto.CardHolderName
        };
        
        var isPaymentSuccess = this.crediCardService.ProcessPaymentAsync(payment);
        if (isPaymentSuccess)
        {
            card.isPaid = true;
          
            var rezervation = new Rezervations()
            {
                CampsiteId = card.CampsiteId,
                CustomerId = card.UserInfoID,
                StartDate = card.StartDate,
                EndDate = card.EndDate,
                NumOfAdult = card.NumOfAdult,
                NumOfChilder = card.NumOfChilder,
                isPaid = card.isPaid,
                isEnable = card.isEnable
            };
            this.context.Rezervations.Add(rezervation);
            await this.context.SaveChangesAsync();
            return true;
        }
        return false;
    }

/*
    public async Task<int> AddCustomerInfo(CreateCardDTO cardDto, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var card = await this.cardService.AddToCardAsync(cardDto, userId);
        return card.Id;
    }
    
    public async Task<bool> ProcessPaymentAsync(PaymentDTO payment, int cardId)
    {
        var isPaymentSuccess = this.crediCardService.ProcessPaymentAsync(payment);
        if (isPaymentSuccess)
        {
          var card = await this.cardService.GetCard(cardId);
          card.isPaid = true;
          
          var rezervation = new Rezervations()
            {
                CampsiteId = card.CampsiteId,
                CustomerId = card.UserInfoID,
                StartDate = card.StartDate,
                EndDate = card.EndDate,
                NumOfAdult = card.NumOfAdult,
                NumOfChilder = card.NumOfChilder,
                isPaid = card.isPaid,
                isEnable = card.isEnable
            };
            this.context.Rezervations.Add(rezervation);
            await this.context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    */ 
    public async Task<List<GetUserReservedModel>> GetUserRezervedCampsite(string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var rezervations = await this.context.Rezervations
            .Where(r => r.CustomerId == userId)
            .ToListAsync();

        var getUserReservedModels = new List<GetUserReservedModel>();

        foreach (var rezervation in rezervations)
        {
            var campsite = await this.context.Campsites
                .FirstOrDefaultAsync(c => c.CampsiteId == rezervation.CampsiteId);

            if (campsite != null)
            {
                var getUserReservedModel = new GetUserReservedModel
                {
                    Campsite = campsite,
                    StartDate = rezervation.StartDate,
                    EndDate = rezervation.EndDate,
                    NumOfAdult = rezervation.NumOfAdult,
                    NumOfChilder = rezervation.NumOfChilder,
                    isPaid = rezervation.isPaid
                };

                getUserReservedModels.Add(getUserReservedModel);
            }
        }
        return getUserReservedModels;
    }
}