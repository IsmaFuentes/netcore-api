namespace netcore_api.Middleware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) 
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
          Exceptions.NotFoundException => StatusCodes.Status404NotFound,
          Exceptions.UserAlreadyExistsException => StatusCodes.Status409Conflict,
          ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
          status = context.Response.StatusCode,
          message = ex.Message
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
      }
    }
  }
}
