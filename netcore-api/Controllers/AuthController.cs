using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace netcore_api.Controllers
{
  [ApiController, Route("api/[controller]"), Authorize]
  public class AuthController : ControllerBase
  {
    private readonly Contracts.Services.IAuthService _authService;

    public AuthController(Contracts.Services.IAuthService authService)
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

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
      if(int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId))
      {
        var authResult = await _authService.RefreshToken(userId);

        return Ok(authResult);
      }

      return Unauthorized();
    }
  }
}
