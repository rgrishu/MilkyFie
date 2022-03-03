﻿using Milkyfie.Models;
using System;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IResponse<T>
    {
        ResponseStatus StatusCode { get; set; }
        string ResponseText { get; set; }
        Exception Exception { get; set; }
        T Result { get; set; }
    }

    public interface IResponse
    {
        ResponseStatus StatusCode { get; set; }
        string ResponseText { get; set; }
        Exception Exception { get; set; }
    }

    public interface IRequest<T>
    {
        string AuthToken { get; set; }
        T Param { get; set; }
    }
}
