using Microsoft.AspNetCore.Identity;
using netcore_api.Contracts.Services;
using netcore_api.Data.Repositories;
using netcore_api.Services.Jwt;

namespace netcore_api.Services
{
  public class AuthService : IAuthService
  {
    private readonly IUserRepository _repository;
    private readonly IJwtTokenService _tokenService;
    private readonly IPasswordHasher<Data.Entities.User> _hasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
      IUserRepository userRepository, 
      IJwtTokenService tokenService,
      IPasswordHasher<Data.Entities.User> hasher, ILogger<AuthService> logger)
    {
      _repository = userRepository;
      _tokenService = tokenService;
      _hasher = hasher;
      _logger = logger;
    }

    public async Task<Contracts.DTO.AuthResponseDto?> Login(Contracts.DTO.AuthRequestDto request)
    {
      var user = await _repository.GetAsync(request.UserName);

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
      var user = await _repository.GetAsync(userId);

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
