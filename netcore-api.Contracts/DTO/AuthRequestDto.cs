using System.ComponentModel.DataAnnotations;

namespace netcore_api.Contracts.DTO
{
  public class AuthRequestDto
  {
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
  }
}
