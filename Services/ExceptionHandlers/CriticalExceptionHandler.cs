﻿
using App.Services.ExceptionHandlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace Clean.API.ExceptionHandlers
{
    public class CriticalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if( exception is CriticalException)
            {
                Console.WriteLine("Sms was sended");
            }

            // i wrapped this exception in the next exception handler .. so i return false 
            return ValueTask.FromResult(false);
        }
    }
}
