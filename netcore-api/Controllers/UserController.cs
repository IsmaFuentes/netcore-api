using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace netcore_api.Controllers
{
  [ApiController, Route("api/[controller]"), Authorize]
  public class UserController : ControllerBase
  {
    private Services.Interfaces.IUserService _userService;
    public UserController(Services.Interfaces.IUserService userService) 
    { 
      _userService = userService;
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
    {
      var result = await _userService.GetUsersAsync(page: page, pageSize: pageSize);

      return Ok(result);
    }

    [HttpGet("{id}"), Authorize(Roles = "Admin,Moderator,User")]
    public async Task<IActionResult> Get(int id)
    {
      var user = await _userService.GetUserAsync(id);

      if(user is not null)
      {
        return Ok(user);
      }

      return NotFound();
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] Contracts.DTO.UserRegistrationDto user)
    {
      var newUser = await _userService.CreateUserAsync(user);

      return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] Contracts.DTO.UserDto user)
    {
      var updated = await _userService.UpdateUserAsync(user);

      return Ok(updated);
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
      var updated = await _userService.DeleteUserAsync(id);

      return NoContent();
    }
  }
}
