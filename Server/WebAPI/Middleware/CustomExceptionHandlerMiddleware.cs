using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using BLL.Exceptions;
using BLL.Models;

namespace WebAPI.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var code = HttpStatusCode.InternalServerError;
            string message;
            switch (exception)
            {
                case GameStoreException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case AuthenticationException:
                    code = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                default:
                    message = exception.Message;
                    break;
            }
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = (int)code,
                Message = message
            }.ToString());
        }
    }
}
