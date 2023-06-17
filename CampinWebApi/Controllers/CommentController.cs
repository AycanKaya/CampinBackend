using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO.CommentDTO;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers;

[Route("[controller]")]
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
    public async Task<IActionResult> DeleteComment(int commentID)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString();
            var isDeleted = await commentService.DeleteComment(commentID, token);
            var resp = new BaseResponseModel<bool>(isDeleted,"Comment deleted successfully.");
            return new OkObjectResult(resp);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }

    [HttpPost("ShareComment")]
    public async Task<IActionResult> ShareComment(UserCampFeedbackModel feedback)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString();
            await commentService.CreateCommentAndRate(feedback, token);
            var resp = new BaseResponseModel<bool>(true,"Comment created successfully.");
            return new OkObjectResult(resp);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
    [HttpPost("UpdateCommentAndRate")]
    public async Task<IActionResult> UpdateCommentAndRate(UpdateUserFeedbackDTO feedback)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString();
            await commentService.UpdateCommentAndRate(feedback, token);
        
            var baseResponseModel = new BaseResponseModel<bool>(true, "Comment updated successfully.");
            return new OkObjectResult(baseResponseModel);
        }
        catch (ValidationException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
            return new BadRequestObjectResult(response);
        }
        catch (FileNotFoundException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
            return new NotFoundObjectResult(response);
        }
        catch(BadHttpRequestException exception)
        {
            var response = new ErrorResponseModel(exception.Message,"Bad Request" , exception.StatusCode);
            return new BadRequestObjectResult(response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return new UnauthorizedObjectResult(exception.Message);
        }
        catch (Exception)
        {
            return new InternalServerErrorResult();
        }
    }
    
}