using _4Tech._4Manager.API.Errors;
using _4Tech._4Manager.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace _4Tech._4Manager.API.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";

                object responseToSerialize;

                if (ex is ValidationException validationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    var validationErrors = validationException.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    responseToSerialize = new ApiException(
                        statusCode: context.Response.StatusCode.ToString(),
                        message: "Falha na validação",
                        details: string.Join("; ", validationErrors)
                    );
                }
                else if (ex is GuidFoundException notFoundEx)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    responseToSerialize = new ApiException(
                        statusCode: context.Response.StatusCode.ToString(),
                        message: notFoundEx.Message,
                        details: "Recurso não encontrado"
                    );
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    responseToSerialize = _env.IsDevelopment()
                        ? new ApiException(context.Response.StatusCode.ToString(), ex.Message, ex.StackTrace?.ToString())
                        : new ApiException(context.Response.StatusCode.ToString(), ex.Message, "Internal Server Error");
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(responseToSerialize, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
