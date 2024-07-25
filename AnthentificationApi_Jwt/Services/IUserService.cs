using AnthentificationApi_Jwt.Models;

namespace AnthentificationApi_Jwt.Services;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationModel> RefreshTokenAsync(string token);
    ApplicationUser GetById(string id);
    Task<AuthenticationModel> Login(LoginModel model);
    Task<bool> RevokeToken(string token);
}
