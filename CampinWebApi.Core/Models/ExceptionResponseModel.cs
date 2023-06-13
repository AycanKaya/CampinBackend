using System;
using System.Globalization;

namespace CampinWebApi.Core.Models
{
    public class ExceptionResponse : Exception
    {
        public ExceptionResponse() : base() { }

        public ExceptionResponse(string message) : base(message) { }

        public ExceptionResponse(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}

