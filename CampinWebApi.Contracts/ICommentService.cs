using CampinWebApi.Core.DTO.CommentDTO;
using CampinWebApi.Domain.Entities;

namespace CampinWebApi.Contracts;

public interface ICommentService
{
    Task<bool> CreateCommentAndRate(UserCampFeedbackModel userCampFeedbackModel, string userToken); 
    Task<bool> DeleteComment(int commentId, string userToken);
    Task<bool> UpdateCommentAndRate(UpdateUserFeedbackDTO updateUserFeedback, string userToken);
}