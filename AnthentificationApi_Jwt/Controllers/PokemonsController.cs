using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnthentificationApi_Jwt.Contexts;
using AnthentificationApi_Jwt.Models;
using AnthentificationApi_Jwt.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AnthentificationApi_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPokemonService _pokeService;

        public PokemonsController(ApplicationDbContext context, IPokemonService pokeService)
        {
            _context = context;
            _pokeService = pokeService;
        }

        // GET: api/Pokemons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonResponse>>> GetPokemons()
        {
            var pokemonlst = await _pokeService.GetPokemonList();
            if (pokemonlst == null) 
                return NotFound();

            var result = pokemonlst.Select(p => new PokemonResponse
            {
                Id = p.Id,
                Name = p.Name,
                Cp = p.Cp,
                Hp = p.Hp,
                Picture = p.Picture,
                Created = p.Created,
                Types = p.Types.Select(t => t.Name).ToList()

            });
            return Ok(result);
        }

        //[HttpGet("{name}")]     
        //public async Task<ActionResult<IEnumerable<PokemonResponse>>> GetPokemonByNameContains(string name)
        //{
        //    var pokemons = await _pokeService.GetPokemonByName(name);
        //    if(pokemons == null)
        //        return NotFound();

        //    var result = pokemons.Select(p => new PokemonResponse
        //    {
        //        Id = p.Id,
        //        Name = p.Name,
        //        Cp = p.Cp,
        //        Hp = p.Hp,
        //        Picture = p.Picture,
        //        Created = p.Created,
        //        Types = p.Types.Select(t => t.Name).ToList()

        //    });
        //    return Ok(result);
        //}

        // GET: api/Pokemons/5
        [HttpGet("{id}")]     
        public async Task<ActionResult<PokemonResponse>> GetPokemon(int id)
        {
            //var model = await _context.Pokemons.FindAsync(id);
            var pokemon = await _pokeService.GetPokemonById(id);

            if (pokemon == null)            
                return NotFound();

            var result = new PokemonResponse
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Cp = pokemon.Cp,
                Hp = pokemon.Hp,
                Picture = pokemon.Picture,
                Created = pokemon.Created,
                Types = pokemon.Types.Select(t => t.Name).ToList()

            };
            return Ok(result);
        }

        // PUT: api/Pokemons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public async Task<IActionResult> PutPokemon(int id, PokemonModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            //_context.Entry(model).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                var result = await _pokeService.UpdatePokemon(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pokemons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PokemonResponse>> PostPokemon(PokemonModel model)
        {
           
            var pokemon = await _pokeService.CreatePokemon(model);
            var resultModel = new PokemonResponse
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Hp = pokemon.Hp,
                Cp = pokemon.Cp,
                Picture = pokemon.Picture,
                Created = pokemon.Created,
                Types = pokemon.Types.Select(t => t.Name).ToList()
            };


            return CreatedAtAction("GetPokemon", new { id = resultModel.Id }, resultModel);
        }

       
        // DELETE: api/Pokemons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            _context.Pokemons.Remove(pokemon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PokemonExists(int id)
        {
            return _context.Pokemons.Any(e => e.Id == id);
        }
    }
}
