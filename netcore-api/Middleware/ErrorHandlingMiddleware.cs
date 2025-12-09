using Microsoft.AspNetCore.Mvc;
using netcore_api.Exceptions;

namespace netcore_api.Middleware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) 
    {
      _next = next;
      _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch(Exception ex)
      {
        _logger.LogError(ex, "Unhandled exception");

        if (context.Response.HasStarted)
        {
          _logger.LogWarning("Cannot write error response because response has already started");
          throw;
        }

        int statusCode = ex switch
        {
          NotFoundException          => StatusCodes.Status404NotFound,
          UserAlreadyExistsException => StatusCodes.Status409Conflict,
          ArgumentException          => StatusCodes.Status400BadRequest, 
          _                          => StatusCodes.Status500InternalServerError
        };

        var response = new ProblemDetails()
        {
          Status = statusCode,
          Title = ex.GetType().Name,
          Type = GetStatusCodeErrorType(statusCode),
          Detail = statusCode == 500 ? "An unexpected error occurred." : ex.Message,
        };
        
        response.Extensions.Add(new KeyValuePair<string, object?>("traceId", System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(response);
      }
    }

    private string GetStatusCodeErrorType(int statusCode)
    {
      return statusCode switch
      {
        400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        500 => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
        _ => $"about:blank"
      };
    }
  }
}
