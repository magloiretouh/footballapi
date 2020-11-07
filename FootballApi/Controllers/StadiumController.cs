using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApi.Models;

namespace FootballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly FootballApiContext _context;

        public StadiumController(FootballApiContext context)
        {
            _context = context;
        }

        // GET: api/Stadium
        [HttpGet]
        public IEnumerable<Stadium> GetStadium()
        {
            return _context.Stadium;
        }

        // GET: api/Stadium/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStadium([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stadium = await _context.Stadium.FindAsync(id);

            if (stadium == null)
            {
                return NotFound();
            }

            return Ok(stadium);
        }

        // PUT: api/Stadium/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStadium([FromRoute] int id, [FromBody] Stadium stadium)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stadium.StadiumID)
            {
                return BadRequest();
            }

            _context.Entry(stadium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StadiumExists(id))
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

        // POST: api/Stadium
        [HttpPost]
        public async Task<IActionResult> PostStadium([FromBody] Stadium stadium)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Stadium.Add(stadium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStadium", new { id = stadium.StadiumID }, stadium);
        }

        // DELETE: api/Stadium/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStadium([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stadium = await _context.Stadium.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }

            _context.Stadium.Remove(stadium);
            await _context.SaveChangesAsync();

            return Ok(stadium);
        }

        private bool StadiumExists(int id)
        {
            return _context.Stadium.Any(e => e.StadiumID == id);
        }
    }
}