using System.Net;

namespace CampinWebApi.Core.Models;

public class ErrorResponseModel
{ 
    
    public ErrorResponseModel(string message, string errors, int statusCode)
    {
        isSuccedd = false;
        Message = message;
        Errors = errors;
        StatusCode = statusCode;
    }
    
    public bool isSuccedd { get; set; }
    public string Message { get; set; }
    public string Errors { get; set; }
    public int StatusCode { get; set; }
    
    
}