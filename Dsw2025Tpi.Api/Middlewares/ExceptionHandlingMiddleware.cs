using Dsw2025Tpi.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Dsw2025Tpi.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                EntityNotFoundException => (int)HttpStatusCode.NotFound,
                InactiveEntityException => (int)HttpStatusCode.UnprocessableEntity,
                SameStateException => (int)HttpStatusCode.UnprocessableEntity,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                DuplicatedEntityException => (int)HttpStatusCode.Conflict,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = ex.Message
            };

            var json = JsonSerializer.Serialize(response); // Serializa el objeto de respuesta a JSON
            await context.Response.WriteAsync(json); // Escribe el JSON directamente en el cuerpo (Body) de la respuesta HTTP.
        }
    }
}
