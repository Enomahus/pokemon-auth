
namespace AnthentificationApi_Jwt.Models;

public class Pokemon
{
        
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Hp { get; set; }
    public int Cp { get; set; }
    public required string Picture { get; set; }
    public DateTime Created { get; set; }
    public virtual ICollection<Types> Types { get; set; } = new List<Types>();
    
         
}
