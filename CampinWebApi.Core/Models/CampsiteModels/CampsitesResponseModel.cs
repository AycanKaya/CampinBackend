using CampinWebApi.Core.Models.CommentModels;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Core.Models.CampsiteModels;

public class CampsitesResponseModel
{
    public CampsiteResponseModel Campsite { get; set; }
    public GetCommentModel[] Comments { get; set; }
    public string[] ImageUrls { get; set; }
    public Features Features { get; set; }
}