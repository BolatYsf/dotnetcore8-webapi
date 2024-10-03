using App.Application;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;


namespace Clean.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var errorDto = ServiceResult.Fail(exception.Message,HttpStatusCode.InternalServerError);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(errorDto, cancellationToken);

            return true;


        }
    }
}
