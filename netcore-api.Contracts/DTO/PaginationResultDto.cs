
namespace netcore_api.Contracts.DTO
{
  public class PaginationResultDto
  {
    public int Page { get; set; }
    public int Count { get; set; }
    public IEnumerable<object> Results { get; set; } = [];
  }
}
