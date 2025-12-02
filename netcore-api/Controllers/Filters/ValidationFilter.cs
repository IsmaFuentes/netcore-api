using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace netcore_api.Controllers.Filters
{
  public class ValidationFilter : IActionFilter
  {
    public void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        var errors = context.ModelState.Where(e => e.Value.Errors.Count > 0).Select(e => new
        {
          Field = e.Key,
          Errors = e.Value.Errors.Select(er => er.ErrorMessage)
        });

        context.Result = new BadRequestObjectResult(new
        {
          Message = "Validation failed",
          Errors = errors,
        });
      }
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    { }
  }
}
