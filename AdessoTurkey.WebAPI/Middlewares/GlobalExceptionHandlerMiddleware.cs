using AdessoTurkey.Application.DTOs;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace AdessoTurkey.WebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ValidationException validationException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = BaseResponse<object>.FailureResult(
                        "Validasyon hatası",
                        validationException.Errors.Select(e => e.ErrorMessage).ToList()
                    )
                },
                ArgumentException argumentException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = BaseResponse<object>.FailureResult(
                        "Geçersiz istek",
                        argumentException.Message
                    )
                },
                InvalidOperationException invalidOperationException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = BaseResponse<object>.FailureResult(
                        "İşlem hatası",
                        invalidOperationException.Message
                    )
                },
                _ => new
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    response = BaseResponse<object>.FailureResult(
                        "Sunucu hatası",
                        "Beklenmeyen bir hata oluştu"
                    )
                }
            };

            context.Response.StatusCode = response.statusCode;

            var jsonResponse = JsonSerializer.Serialize(response.response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
