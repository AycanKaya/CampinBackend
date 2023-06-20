using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CardDTO;
using CampinWebApi.Core.DTO.PaymentDTO;
using CampinWebApi.Core.DTO.RezervationDTO;
using CampinWebApi.Core.Models.CampsiteModels;
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
    
    public async Task<List<GetUserReservedModel>> GetUserRezervedCampsite(string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var user = await context.UserInfo.FirstOrDefaultAsync(x => x.UserID == userId);
        
        var rezervations = await this.context.Rezervations
            .Where(r => r.CustomerId == userId)
            .ToListAsync();

        var getUserReservedModels = new List<GetUserReservedModel>();

        foreach (var rezervation in rezervations)
        {
            var campsite = await this.context.Campsites
                .FirstOrDefaultAsync(c => c.CampsiteId == rezervation.CampsiteId);

            var holidayDestination =
                await context.HolidayDestinations.FirstOrDefaultAsync(x => x.Id == campsite.HolidayDestinationId);
            
            var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == holidayDestination.CityId);
            
            // check user comment is exist
            var userCommentIsExist = await context.Comments
                .AnyAsync(x => x.CampsiteId == campsite.CampsiteId && x.AuthorId == userId);

            if (campsite != null)
            {
                var getUserReservedModel = new GetUserReservedModel
                {
                    Campsite = new CampsiteResponseModel
                    {
                        Capacity = campsite.Capacity,
                        CampsiteId = campsite.CampsiteId,
                        Description = campsite.Description,
                        HolidayDestinationName = holidayDestination.HolidayDestinationName,
                        CityName = city.CityName,
                        OwnerId = userId,
                        OwnerName = user.Name,
                        OwnerSurname = user.Surname,
                        AdultPrice = campsite.AdultPrice,
                        ChildPrice = campsite.ChildPrice,
                        OwnerEmail = user.Email,
                        OwnerPhoneNumber = user.PhoneNumber,
                        Rate = campsite.Rate,
                        SeasonCloseDate = campsite.SeasonCloseDate,
                        SeasonStartDate = campsite.SeasonStartDate,
                        lat = campsite.lat,
                        lng = campsite.lng,
                        Name = campsite.Name
                    },
                    rezervationStartDate = rezervation.StartDate,
                    rezervationEndDate = rezervation.EndDate,
                    NumOfAdult = rezervation.NumOfAdult,
                    NumOfChilder = rezervation.NumOfChilder,
                    isCommentExist = userCommentIsExist
                };
                getUserReservedModels.Add(getUserReservedModel);
            }
        }
        return getUserReservedModels;
    }
}