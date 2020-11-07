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
    public class FootballTeamsController : ControllerBase
    {
        private readonly FootballApiContext _context;

        public FootballTeamsController(FootballApiContext context)
        {
            _context = context;
        }

        // GET: api/FootballTeams
        [HttpGet]
        public IEnumerable<FootballTeam> GetFootballTeam()
        {
            return _context.FootballTeam;
        }

        // GET: api/FootballTeams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFootballTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var footballTeam = await _context.FootballTeam.FindAsync(id);

            if (footballTeam == null)
            {
                return NotFound();
            }

            return Ok(footballTeam);
        }

        // PUT: api/FootballTeams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFootballTeam([FromRoute] int id, [FromBody] FootballTeam footballTeam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != footballTeam.FootballTeamID)
            {
                return BadRequest();
            }

            _context.Entry(footballTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FootballTeamExists(id))
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

        // POST: api/FootballTeams
        [HttpPost]
        public async Task<IActionResult> PostFootballTeam([FromBody] FootballTeam footballTeam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FootballTeam.Add(footballTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFootballTeam", new { id = footballTeam.FootballTeamID }, footballTeam);
        }

        // DELETE: api/FootballTeams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFootballTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var footballTeam = await _context.FootballTeam.FindAsync(id);
            if (footballTeam == null)
            {
                return NotFound();
            }

            _context.FootballTeam.Remove(footballTeam);
            await _context.SaveChangesAsync();

            return Ok(footballTeam);
        }

        private bool FootballTeamExists(int id)
        {
            return _context.FootballTeam.Any(e => e.FootballTeamID == id);
        }
    }
}