namespace AnthentificationApi_Jwt.Models;

public class Types
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
    //public virtual ICollection<PokemonType> PokemonTypes { get; set; } = new List<PokemonType>();
}
