using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using netcore_api.Data.Repositories;
using Moq;

namespace netcore_api.Test
{
  public class UnitTesting
  {
    [Fact]
    public async Task Test_GetUserAsync()
    {
      var options = new DbContextOptionsBuilder<Data.Context>()
        .UseInMemoryDatabase(databaseName: "TestingDb").Options;

      var context = new Data.Context(options);

      context.Users.Add(new Data.Entities.User()
      {
        UserName = "Administrator",
        Password = "password",
        Role = Data.Entities.UserRole.Admin,
        IsActive = true,
      });

      await context.SaveChangesAsync();

      var logger = new Mock<ILogger<Services.UserService>>();
      var hasher = new Mock<IPasswordHasher<Data.Entities.User>>();

      var service = new Services.UserService(new UserRepository(context), hasher.Object, logger.Object);

      var result = await service.GetUserAsync(1);
      Assert.NotNull(result);
      Assert.Equal("Administrator", result.UserName);
    }
  }
}
