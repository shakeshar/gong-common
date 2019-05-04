using System;
using System.Collections.Generic;
using System.Text;

namespace Gong.Common.Core.CQRS
{
    public abstract class Result
    {           
        public string Message { get; }
        public bool IsSuccess { get; }
        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public Result(bool isSuccess, string message):this(isSuccess)
        {
            this.Message = message;
        }
        public virtual object Build()
        {
            return this;
        }   
    }
    public class SuccessResult : Result
    {
        public SuccessResult(string message = ""):base(true, message)
        {
        }        
    }
    public class SuccessResult<T> : Result
    { 
        public T Data { get; private set; }
        public SuccessResult(T data, string message = "") : base(true, message)
        {
            Data = data;
        }
        public override object Build()
        {
            return Data;
        }
    }

    public class ErrorResult : Result
    {       
        public ErrorResult(string message = "") : base(false, message)
        {

        }
    }
}
