using System.ComponentModel.DataAnnotations;

namespace AnthentificationApi_Jwt.Models;

public class AddRoleModel
{
    
    public required string Email { get; set; }     
    public required string Password { get; set; }    
    public required string Role { get; set; }
}
