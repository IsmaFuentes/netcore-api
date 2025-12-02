using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace netcore_api.Controllers
{
  [ApiController, Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly Services.Interfaces.IAuthService _authService;

    public AuthController(Services.Interfaces.IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Contracts.DTO.AuthRequestDto auth)
    {
      var authResult = await _authService.Login(auth);

      if (authResult is not null)
      {
        return Ok(authResult);
      }

      return Unauthorized();
    }
  }
}
