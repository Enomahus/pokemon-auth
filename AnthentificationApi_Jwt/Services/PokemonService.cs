using AnthentificationApi_Jwt.Contexts;
using AnthentificationApi_Jwt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AnthentificationApi_Jwt.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly ApplicationDbContext _context;

        public PokemonService(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<Pokemon> CreatePokemon(PokemonModel model)
        {
            var pokemon = new Pokemon
            { 
                Name = model.Name,
                Hp = model.Hp,
                Cp = model.Cp,
                Picture = model.Picture,
                Created = model.Created,
                Types = new List<Models.Types>()
            };
                       

            foreach (var item in model.Types)
            {
                var types = await _context.Types.FirstOrDefaultAsync(t => t.Name == item);
                if (types == null)
                {
                    var type = new Models.Types { Name = item, Pokemons = new List<Pokemon> { pokemon } };                   
                    pokemon.Types.Add(type);
                }
                else
                    pokemon.Types.Add(types);
            }

            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();

            return pokemon;
        }

        public async Task<Pokemon> AddPokemonWithType(PokemonModel model) 
        {

            var pokemon = new Pokemon
            {
                Name = model.Name,
                Hp = model.Hp,
                Cp = model.Cp,
                Picture = model.Picture,
                Types = new List<Models.Types>(),
                Created = model.Created,
            };

            foreach (var item in model.Types)
            {
                var types = await _context.Types.FirstOrDefaultAsync(t => t.Name == item);
                if(types != null)
                    pokemon.Types.Add(new Models.Types { Name = item });
                else
                {
                    var type = new Models.Types { Name = item, Pokemons = new List<Pokemon> { pokemon } };
                    pokemon.Types.Add(type);
                }
            }          
            
            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();

            return pokemon;

           

        }

        public async Task DeletePokemon(int id)
        {
            var pokemonToDelete = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id)
               ?? throw new Exception($"Pokemon {id} not found");

            _context.Pokemons.Remove(pokemonToDelete);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Pokemon> GetPokemonById(int id)
        {
            return await _context.Pokemons
                .Include(p => p.Types)
                .FirstOrDefaultAsync(p => p.Id == id);
                
        }

        public async Task<List<Pokemon>> GetPokemonByName(string name)
        {
            return await _context.Pokemons.Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Pokemon>> GetPokemonList()
        {
            return await _context.Pokemons.Include(p => p.Types).ToListAsync();
        }

        public async Task<Pokemon> UpdatePokemon(PokemonModel model)
        {
            var pokemonToUpdate = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == model.Id)
                ?? throw new Exception($"Pokemon {model.Id} not found");
            pokemonToUpdate.Types = new List<Models.Types>();
            //pokemonToUpdate.PokemonTypes = new List<PokemonType>();

            //pokemonToUpdate.Id = model.Id;
            pokemonToUpdate.Name = model.Name;
            pokemonToUpdate.Hp = model.Hp;
            pokemonToUpdate.Cp = model.Cp;
            pokemonToUpdate.Picture = model.Picture;
            pokemonToUpdate.Created = model.Created;

            foreach (var item in model.Types)
            {
                var type = await _context.Types.Include(t => t.Pokemons).FirstOrDefaultAsync(t => t.Name == item);
                if (type != null)                
                    pokemonToUpdate.Types.Add(type);
                else
                {
                    var typeNew = new Models.Types { Name = item, Pokemons = new List<Pokemon> { pokemonToUpdate } };
                    _context.Types.Add(typeNew);
                    await _context.SaveChangesAsync();

                    pokemonToUpdate.Types.Add(typeNew);
                }

                
            }

            _context.Pokemons.Update(pokemonToUpdate);
            await _context.SaveChangesAsync();

            return pokemonToUpdate;              

        }
    }
}
