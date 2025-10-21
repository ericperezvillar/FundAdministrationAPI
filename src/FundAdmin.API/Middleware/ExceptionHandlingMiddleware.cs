using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace FundAdmin.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ProblemDetailsFactory _pdf;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ProblemDetailsFactory pdf,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _pdf = pdf;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            // Handle common framework-generated responses (401, 403)
            if (!context.Response.HasStarted)
            {
                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.Unauthorized:
                        await WriteProblemAsync(context, HttpStatusCode.Unauthorized,
                            "Unauthorized", "You must provide a valid JWT token to access this resource.");
                        break;

                    case (int)HttpStatusCode.Forbidden:
                        await WriteProblemAsync(context, HttpStatusCode.Forbidden,
                            "Forbidden", "You do not have permission to access this resource.");
                        break;
                }
            }
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Bad Request: {Message}", ex.Message);
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Bad Request", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Not Found: {Message}", ex.Message);
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, "Unauthorized", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "Internal Server Error", "An unexpected error occurred while processing your request.");
        }
    }

    private async Task WriteProblemAsync(HttpContext context, HttpStatusCode code, string title, string detail)
    {
        if (context.Response.HasStarted)
            return;

        var problem = _pdf.CreateProblemDetails(context,
            statusCode: (int)code,
            title: title,
            detail: detail);

        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    }
}
