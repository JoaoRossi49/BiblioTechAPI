using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiblioTechAPI.Models;

namespace BiblioTechAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {
        private readonly BibliotechContext _dbContext;

        public GenerosController(BibliotechContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> ObterGeneros()
        {
            var generos = await _dbContext.Generos.ToListAsync();
            return Ok(generos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> ObterGenero(int id)
        {
            var genero = await _dbContext.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return Ok(genero);
        }

        [HttpPost]
        public async Task<ActionResult<Genero>> CriarGenero(Genero genero)
        {
            _dbContext.Generos.Add(genero);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterGenero), new { id = genero.IdGenero }, genero);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarGenero(int id, Genero genero)
        {
            if (id != genero.IdGenero)
            {
                return BadRequest();
            }

            _dbContext.Entry(genero).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirGenero(int id)
        {
            var genero = await _dbContext.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }

            _dbContext.Generos.Remove(genero);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroExists(int id)
        {
            return _dbContext.Generos.Any(g => g.IdGenero == id);
        }
    }
}
