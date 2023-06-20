using CampinWebApi.Core.Models.CampsiteModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Core.Models.RezervationsModel;

public class GetUserReservedModel
{
    public CampsiteResponseModel Campsite { get; set; }
    public DateTime rezervationStartDate { get; set; }
    public DateTime rezervationEndDate { get; set; }
    public int NumOfAdult { get; set; }
    public int NumOfChilder { get; set; }
    public bool isCommentExist { get; set; }
}