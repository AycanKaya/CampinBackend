using System.Net;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CommentDTO;
using CampinWebApi.Core.Models;
using CampinWebApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService commentService;

    public CommentController(ICommentService commentService)
    {
        this.commentService = commentService;
    }
    
    [HttpDelete("DeleteComment")]
    public async Task<BaseResponseModel<bool>> DeleteComment(int commentID)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString();
        var isDeleted = await commentService.DeleteComment(commentID, token);
        
        if (isDeleted)
            return new BaseResponseModel<bool>(isDeleted,"Comment deleted successfully.");
        
        return new BaseResponseModel<bool>(isDeleted,"Comment not found.");
    }

    [HttpPost("ShareComment")]
    public async Task<BaseResponseModel<bool>> ShareComment(UserCampFeedbackModel feedback)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString();
        await commentService.CreateCommentAndRate(feedback, token);
        
        return new BaseResponseModel<bool>(true, "Comment created successfully.");
    }
    
    [HttpPost("UpdateCommentAndRate")]
    public async Task<BaseResponseModel<bool>> UpdateCommentAndRate(UpdateUserFeedbackDTO feedback)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString();
        await commentService.UpdateCommentAndRate(feedback, token);
        
        return new BaseResponseModel<bool>(true, "Comment updated successfully.");
    }
    
}