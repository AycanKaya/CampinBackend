using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using CampinWebApi.Contracts;
using CampinWebApi.Core.DTO;
using CampinWebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampinWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]   
    public class AccountController : ControllerBase
    {
        IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        
        [HttpPost("authenticate")]
        public async Task<IActionResult> Login(AuthenticationRequestDTO request)
        {
            try
            {
                var user = await accountService.Login(request, GenerateIPAddress());
                var result = new BaseResponseModel<AuthenticationResponseDTO>(user, "Logged in");
                return  new OkObjectResult(result);   
            }
            catch (ValidationException exception)
            {
                var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
                return new BadRequestObjectResult(response);
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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            try
            {
                var isSuccedd = await accountService.Register(request);
                var response = new BaseResponseModel<bool>();
                if (isSuccedd)
                {
                    response.Succeeded = true;
                    response.Message = "User registered successfully.";
                }
                return new OkObjectResult(response);
            }
            catch (ValidationException exception)
            {
                var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
                return new BadRequestObjectResult(response);
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
        
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO request)
        {
            try
            {
                var response = new BaseResponseModel<string>();
                var isSucced = await accountService.ResetPassword(request);
                if (isSucced)
                {
                    response.Succeeded = isSucced;
                    response.Message = "Password Change Successful. ";
                    response.Errors = "No error";
                }

                return new OkObjectResult(response);
            }
            catch (FileNotFoundException exception)
            {
                var response = new ErrorResponseModel(exception.Message,"Object not found", (int)HttpStatusCode.BadRequest);
                return new NotFoundObjectResult(response);
            }
            catch (ValidationException exception)
            {
                var response = new ErrorResponseModel(exception.Message,"Validation Error", (int)HttpStatusCode.BadRequest);
                return new BadRequestObjectResult(response);
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

        [HttpPost]
        [Route("add-role")]
        public async Task<IActionResult> AddRoleToUser(string roleName)
        {
            try
            {
                var userToken = HttpContext.Request.Headers.Authorization.ToString();
                var response = new BaseResponseModel<string>();
                var isSucced = await accountService.AddRoleToUser(userToken, roleName);
                if (isSucced)
                {
                    response.Succeeded = isSucced;
                    response.Message = "Role added to user. ";
                    response.Errors = "No error";
                }
                return new OkObjectResult(response);
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
        private string GetToken()
        {
            return HttpContext.Request.Headers.Authorization.ToString();
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress == null ? "127.0.0.1" : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }

}