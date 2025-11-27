using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netcore_api.Services;

namespace netcore_api.Controllers
{
  [ApiController, Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Contracts.DTO.AuthRequestDto auth)
    {
      if (!string.IsNullOrEmpty(auth.UserName) && 
          !string.IsNullOrEmpty(auth.Password)) 
      {
        var authResult = await _authService.Login(auth);

        if(authResult is not null)
        {
          return Ok(authResult);
        }
      }

      return Unauthorized();
    }
  }
}
