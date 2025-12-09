using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace netcore_api.Services
{
  public class AuthService : Interfaces.IAuthService
  {
    private readonly Data.Context _ctx;
    private readonly Interfaces.IJwtTokenService _tokenService;
    private readonly IPasswordHasher<Data.Entities.User> _hasher;

    public AuthService(Data.Context context, Interfaces.IJwtTokenService tokenService, IPasswordHasher<Data.Entities.User> hasher)
    {
      _ctx = context;
      _tokenService = tokenService;
      _hasher = hasher;
    }

    public async Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto request)
    {
      var user = await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(e => e.UserName == request.UserName);

      if (user is null)
        throw new Exceptions.NotFoundException("user not found");

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

      return default;
    }

    public async Task<Contracts.DTO.AuthResponseDto> RefreshToken(int userId)
    {
      var user = await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == userId);

      if (user is null)
        throw new Exceptions.NotFoundException("user not found");

      if (!user.IsActive)
        throw new Exceptions.UnauthorizedException("the user is inactive");

      return new Contracts.DTO.AuthResponseDto 
      { 
        Token = _tokenService.GenerateToken(user) 
      };
    }
  }
}
