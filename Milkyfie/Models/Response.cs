using Milkyfie.AppCode.Interfaces;
using System;

namespace Milkyfie.Models
{
    public class Response<T> : IResponse<T>
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }
    }

    public class Response : IResponse
    {
        public ResponseStatus StatusCode { get; set; }
        public string ResponseText { get; set; }
        public Exception Exception { get; set; }
    }

    public class Request<T> : IRequest<T>
    {
        public string AuthToken { get; set; }
        public T Param { get; set; }
    }

    public enum ResponseStatus
    {
        Failed = -1,
        Success = 1,
        Pending = 2,
        info = 3,
        warning = 4,
    }
}
