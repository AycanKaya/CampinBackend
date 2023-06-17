using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CommentDTO;
using CampinWebApi.Domain;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CampinWebApi.Services;

public class CommentService :ICommentService
{
    private readonly CampinDbContext context;
    private readonly IJWTService jwtService;
    public CommentService(CampinDbContext context, IJWTService jwtService)
    {
        this.context = context;
        this.jwtService = jwtService;
    }
    
    // if user reserved campsite , user can comment on it and user can rate this campsite
    public async Task<bool> CreateCommentAndRate(UserCampFeedbackModel userCampFeedbackModel, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var user = await context.UserInfo.FirstOrDefaultAsync(u => u.UserID == userId);
        
        if (!CheckUserRezervedCampsite(userId, userCampFeedbackModel.CampsiteId).Result)
            throw new BadHttpRequestException("You can not comment and rate on this campsite because you did not reserve it !!");
        
        var comment = new Comments
        {
            AuthorId = userId,
            AuthorName = user.Name,
            CampsiteId = userCampFeedbackModel.CampsiteId,
            Content = userCampFeedbackModel.Comment,
            Created = DateTime.Now,
            IsDeleted = false,
        };
        
        // if user already rated this campsite , update rate
        var oldRate = await context.Ratings.FirstOrDefaultAsync(r => r.CampsiteId == userCampFeedbackModel.CampsiteId && r.UserId == userId);
        
        if (oldRate != null)
        {
            oldRate.Rate = userCampFeedbackModel.Rate;
            context.Ratings.Update(oldRate);
        }
        else
        {
            var rate = new Rating
            {
                RatingId = Guid.NewGuid().ToString(),
                CampsiteId = userCampFeedbackModel.CampsiteId,
                UserId = userId,
                Rate = userCampFeedbackModel.Rate,
            }; 
            
            context.Ratings.Add(rate);
        }
        await context.SaveChangesAsync();

        // Update campsite rating 
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == userCampFeedbackModel.CampsiteId);
        var campsiteRates = await context.Ratings.Where(r => r.CampsiteId == userCampFeedbackModel.CampsiteId).ToListAsync();
        campsite.Rate = campsiteRates.Average(r => r.Rate);
        context.Campsites.Update(campsite);

        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateCommentAndRate(UpdateUserFeedbackDTO updateUserFeedback, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var user = await context.UserInfo.FirstOrDefaultAsync(u => u.UserID == userId);
        
        if (!CheckUserRezervedCampsite(userId, updateUserFeedback.CampsiteId).Result)
            throw new BadHttpRequestException("You can not comment and rate on this campsite because you did not reserve it !!");
        
        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == updateUserFeedback.CommentId);
        
        var rate = await context.Ratings.FirstOrDefaultAsync(r => r.CampsiteId == updateUserFeedback.CampsiteId && r.UserId == userId);
        
        comment.Content = updateUserFeedback.Comment;
        rate.Rate = updateUserFeedback.Rate;
        
        context.Comments.Update(comment);
        context.Ratings.Update(rate);
        
        // Update campsite rating 
        var campsite = await context.Campsites.FirstOrDefaultAsync(c => c.CampsiteId == updateUserFeedback.CampsiteId);
        var campsiteRates = await context.Ratings.Where(r => r.CampsiteId == updateUserFeedback.CampsiteId).ToListAsync();
        campsite.Rate = campsiteRates.Average(r => r.Rate);
        context.Campsites.Update(campsite);
        
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteComment(int commentId, string userToken)
    {
        var userId = jwtService.GetUserIdFromJWT(userToken);
        var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if(comment == null)
            throw new FileNotFoundException("Comment not found !!");

        if (comment.AuthorId != userId)
            throw new BadHttpRequestException("You are not the author of this comment !!");
        
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
        return true;
    }
    
    private async Task<bool> CheckUserRezervedCampsite(string userId, string campsiteId)
    {
        var campsiteIds = await this.context.Rezervations.Where(r => r.CustomerId == userId).Select(r => r.CampsiteId).ToListAsync();
        if (campsiteIds.Contains(campsiteId))
            return true;
        return false;
    }

}