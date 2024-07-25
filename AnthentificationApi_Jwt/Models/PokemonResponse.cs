namespace AnthentificationApi_Jwt.Models;

public class PokemonResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Hp { get; set; }
    public int Cp { get; set; }
    public required string Picture { get; set; }
    public DateTime Created { get; set; }
    public List<string> Types { get; set; } = new List<string>();
}
