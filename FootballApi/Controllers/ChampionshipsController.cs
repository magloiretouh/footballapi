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
    public class ChampionshipsController : ControllerBase
    {
        private readonly FootballApiContext _context;

        public ChampionshipsController(FootballApiContext context)
        {
            _context = context;
        }

        // GET: api/Championships
        [HttpGet]
        public IEnumerable<Championship> GetChampionship()
        {
            return _context.Championship;
        }

        // GET: api/Championships/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChampionship([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var championship = await _context.Championship.FindAsync(id);

            if (championship == null)
            {
                return NotFound();
            }

            return Ok(championship);
        }

        // PUT: api/Championships/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChampionship([FromRoute] int id, [FromBody] Championship championship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != championship.ChampionshipID)
            {
                return BadRequest();
            }

            _context.Entry(championship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChampionshipExists(id))
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

        // POST: api/Championships
        [HttpPost]
        public async Task<IActionResult> PostChampionship([FromBody] Championship championship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Championship.Add(championship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChampionship", new { id = championship.ChampionshipID }, championship);
        }

        // DELETE: api/Championships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChampionship([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var championship = await _context.Championship.FindAsync(id);
            if (championship == null)
            {
                return NotFound();
            }

            _context.Championship.Remove(championship);
            await _context.SaveChangesAsync();

            return Ok(championship);
        }

        private bool ChampionshipExists(int id)
        {
            return _context.Championship.Any(e => e.ChampionshipID == id);
        }
    }
}