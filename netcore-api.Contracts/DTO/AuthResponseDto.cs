namespace netcore_api.Contracts.DTO
{
  public class AuthResponseDto
  {
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; } = DateTime.Now;
  }
}
