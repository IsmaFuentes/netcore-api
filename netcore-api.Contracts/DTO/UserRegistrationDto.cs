using System.ComponentModel.DataAnnotations;

namespace netcore_api.Contracts.DTO
{
  public class UserRegistrationDto
  {
    [Required]
    public int Role { get; set; }

    [Required, MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;
  }
}
