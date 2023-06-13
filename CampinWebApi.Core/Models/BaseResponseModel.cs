using System;
namespace CampinWebApi.Core.Models
{
    public class BaseResponseModel<T>
    {

        public BaseResponseModel()
        {
        }
        public BaseResponseModel(T body, string message = null)
        {
            Succeeded = true;
            Message = message;
            Body = body;
        }
        public BaseResponseModel(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public T Body { get; set; }
    }

}

