using Microsoft.AspNetCore.Mvc;

namespace netcore_api.Controllers
{
  [ApiController, Route("api")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public IActionResult Index()
    {
      return Ok(new
      {
        ServiceName = "MyApi",
        ServiceTime = DateTime.UtcNow,
      });
    }
  }
}
