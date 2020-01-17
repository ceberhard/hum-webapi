using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Hum.Common.Utility;
using Microsoft.AspNetCore.Http;

namespace Hum.WebAPI.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
    //     private readonly ILoggerManager _logger;
    
    //    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
    
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            
            HumAppError apperror = exception as HumAppError;
            if (apperror != null)
            {

                switch (apperror.Type)
                {
                    case HumAppErrorType.RestrictedAccess:
                        context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                        break;
                    case HumAppErrorType.Unauthorized:
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        break;
                    case HumAppErrorType.Unexpected:
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                    case HumAppErrorType.Validation:
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(apperror.Message))
                    return context.Response.WriteAsync(JsonSerializer.Serialize(new 
                    {
                        Message = apperror.Message
                    }));
                else
                    return context.Response.WriteAsync(string.Empty);
            }
            else
                return context.Response.WriteAsync(string.Empty);
        }
    }
}
