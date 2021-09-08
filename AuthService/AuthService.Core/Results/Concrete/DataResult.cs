using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Core.Results.Abstract;

namespace AuthService.Core.Results.Concrete
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success):base(success)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
