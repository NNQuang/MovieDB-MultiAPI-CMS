using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Core.Results.Abstract
{
    public interface IDataResult<out T>:IResult
    {
        T Data { get;}
    }
}
