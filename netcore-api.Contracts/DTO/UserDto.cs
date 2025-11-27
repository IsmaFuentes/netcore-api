using System.Text.Json.Serialization;

namespace netcore_api.Contracts.DTO
{
  public class UserDto
  {
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int Role { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? DeletedAt { get; set; }
  }
}
