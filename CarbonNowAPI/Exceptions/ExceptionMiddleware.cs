using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using CarbonNowAPI.Exceptions;

public class ExceptionMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (ConflictException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Campo, ex.Message);

        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Campo, ex.Message);

        }
        catch (InvalidCredentialException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Login", ex.Message);

        }
        catch (Exception ex)
        {
            _logger.LogError($"---\n---\nLog Middleware, erro inesperado: {ex.Message}\nErro completo: {ex}\n---\n---");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Erro", "Erro interno no servidor.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string campo, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            type = $"https://httpstatuses.com/{(int)statusCode}",
            title = "Um ou mais erros ocorreram",
            status = (int)statusCode,
            errors = new Dictionary<string, string[]> {
                { campo, new[] { message } }
            },
            traceId = Activity.Current?.Id ?? context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
