using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Desafio03.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await EscribirErrorAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await EscribirErrorAsync(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado al procesar la solicitud.");
            await EscribirErrorAsync(context, StatusCodes.Status500InternalServerError,
                "Ocurrió un error interno. Revise los registros de la aplicación.");
        }
    }

    private static async Task EscribirErrorAsync(HttpContext context, int statusCode, string mensaje)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            status = statusCode,
            error = mensaje
        }));
    }
}
