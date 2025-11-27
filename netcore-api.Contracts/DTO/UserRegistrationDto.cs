namespace netcore_api.Contracts.DTO
{
  public class UserRegistrationDto
  {
    public int Role { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  }
}
