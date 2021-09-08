using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Core.Results.Abstract;

namespace AuthService.Core.Results.Concrete
{
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Success = success;
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
