using netcore_api.Data.Entities;
using netcore_api.Contracts.DTO;

namespace netcore_api.Mapping
{
  public static class UserMappingExtensions
  {
    public static UserDto MapToUserDto(this User user)
    {
      return new UserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        RegistrationDate = user.RegistrationDate,
        IsActive = user.IsActive,
        Role = (int)user.Role,
        IsDeleted = user.IsDeleted,
        DeletedAt = user.DeletedAt,
      };
    }
  }
}
