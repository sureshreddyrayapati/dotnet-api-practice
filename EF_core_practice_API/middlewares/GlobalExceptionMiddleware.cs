using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace EF_core_practice_API.middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate requestDelegate,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,
                    "Database error occurred at {Path}",
                    context.Request.Path);

                await HandleExceptionAsync(
                    context,
                    "Database error occurred while adding duplicate record.",
                    HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception occurred at {Path}",
                    context.Request.Path);

                await HandleExceptionAsync(
                    context,
                    "Internal server error.",
                    HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                Success = false,
                Message = message
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}