using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace netcore_api.Services
{
  public class AuthService : Interfaces.IAuthService
  {
    private readonly Data.Context _ctx;
    private readonly Interfaces.IJwtTokenService _tokenService;
    private readonly IPasswordHasher<Data.Entities.User> _hasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
      Data.Context context, 
      Interfaces.IJwtTokenService tokenService,
      IPasswordHasher<Data.Entities.User> hasher, ILogger<AuthService> logger)
    {
      _ctx = context;
      _tokenService = tokenService;
      _hasher = hasher;
      _logger = logger;
    }

    public async Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto request)
    {
      var user = await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(e => e.UserName == request.UserName);

      if (user is null)
      {
        _logger.LogInformation($"user with username '{request.UserName}' not found.");
        throw new Exceptions.NotFoundException("user not found");
      }

      if(user.IsActive)
      {
        if (_hasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success)
        {
          return new Contracts.DTO.AuthResponseDto 
          { 
            Token = _tokenService.GenerateToken(user) 
          };
        }
      }

      _logger.LogInformation($"invalid login attempt for '{request.UserName}'.");

      return default;
    }

    public async Task<Contracts.DTO.AuthResponseDto> RefreshToken(int userId)
    {
      var user = await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == userId);

      if (user is null)
      {
        _logger.LogInformation($"invalid refresh token attempt.");
        throw new Exceptions.NotFoundException("user not found");
      }

      if (!user.IsActive)
        throw new Exceptions.UnauthorizedException("this user has been unauthorized");

      return new Contracts.DTO.AuthResponseDto 
      { 
        Token = _tokenService.GenerateToken(user) 
      };
    }
  }
}
