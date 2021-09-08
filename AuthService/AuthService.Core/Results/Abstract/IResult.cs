using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Core.Results.Abstract
{
    public interface IResult
    {
        bool Success { get; set; }
        string Message { get; set; }
    }
}
