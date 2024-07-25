using AnthentificationApi_Jwt.Entities;
using Microsoft.AspNetCore.Identity;

namespace AnthentificationApi_Jwt.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public List<RefreshToken> RefreshTokens { get; set; }
}
