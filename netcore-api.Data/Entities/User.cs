using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netcore_api.Data.Entities
{
  public enum UserRole : int 
  { 
    User = 1,
    Moderator = 2,
    Admin = 3,
  }

  [Table("AppUser"), 
   PrimaryKey("Id"), 
   Index(nameof(UserName), IsUnique = true)]
  public class User
  {
    public User()
    {
      RegistrationDate = DateTime.Now;
    }

    public int Id { get; private set; }

    [Required, MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public DateTime RegistrationDate { get; private set; }

    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [Required]
    public UserRole Role { get; set; }
  }
}
