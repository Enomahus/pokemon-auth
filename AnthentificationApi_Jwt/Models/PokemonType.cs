namespace AnthentificationApi_Jwt.Models;

public class PokemonType
{

    public int PokemonsId { get; set; }
    public int TypesId { get; set; }
    public Pokemon Pokemons { get; set; }
    public Types Types { get; set; } 

}