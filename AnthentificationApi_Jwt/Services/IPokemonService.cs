using AnthentificationApi_Jwt.Models;

namespace AnthentificationApi_Jwt.Services;

public interface IPokemonService
{
    Task<IEnumerable<Pokemon>> GetPokemonList();
    Task<Pokemon> GetPokemonById(int id);
    Task<List<Pokemon>> GetPokemonByName(string name);
    Task<Pokemon> CreatePokemon(PokemonModel model);
    Task<Pokemon> UpdatePokemon(PokemonModel pokemon);
    Task DeletePokemon(int id);
    Task<Pokemon> AddPokemonWithType(PokemonModel model);

}
