using System.ComponentModel.DataAnnotations;

namespace AnthentificationApi_Jwt.Models;

public class TokenRequestModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
