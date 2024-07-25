using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnthentificationApi_Jwt.Contexts;
using AnthentificationApi_Jwt.Models;

namespace AnthentificationApi_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Types>>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }

        // GET: api/Types/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Types>> GetTypes(int id)
        {
            var types = await _context.Types.FindAsync(id);

            if (types == null)
            {
                return NotFound();
            }

            return types;
        }

        // PUT: api/Types/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypes(int id, Models.Types types)
        {
            if (id != types.Id)
            {
                return BadRequest();
            }

            _context.Entry(types).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypesExists(id))
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

        // POST: api/Types
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Types>> PostTypes(Models.Types types)
        {
            _context.Types.Add(types);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypes", new { id = types.Id }, types);
        }

        // DELETE: api/Types/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypes(int id)
        {
            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }

            _context.Types.Remove(types);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypesExists(int id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
    }
}
