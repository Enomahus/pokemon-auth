using System.Text.Json.Serialization;

namespace AnthentificationApi_Jwt.Models;

public class AuthenticationModel
{
    public string Message { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; }
    public string Token { get; set; } = string.Empty;
    [JsonIgnore]
    public string RefreshToken { get; set; } 
    public DateTime RefreshTokenExpiration { get; set; }
}
