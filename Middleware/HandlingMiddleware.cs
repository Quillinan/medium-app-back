using System.Net;
using System.Text.Json;
using medium_app_back.Models;

namespace medium_app_back.Middleware
{
    public class HandlingMiddleware(RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                code = ((int)code).ToString(),
                message = exception.Message,
                detailedMessage = exception.StackTrace ?? string.Empty,
                helpUrl = "",
                details = (object?)null
            };

            var result = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
