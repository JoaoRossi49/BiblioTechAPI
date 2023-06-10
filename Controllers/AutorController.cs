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
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly BibliotechContext _dbContext;

        public AutoresController(BibliotechContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> ObterAutores()
        {
            var autores = await _dbContext.Autors.ToListAsync();
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> ObterAutor(int id)
        {
            var autor = await _dbContext.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        public async Task<ActionResult<Autor>> CriarAutor(Autor autor)
        {
            _dbContext.Autors.Add(autor);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterAutor), new { id = autor.IdAutor }, autor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAutor(int id, Autor autor)
        {
            if (id != autor.IdAutor)
            {
                return BadRequest();
            }

            _dbContext.Entry(autor).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
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
        public async Task<IActionResult> ExcluirAutor(int id)
        {
            var autor = await _dbContext.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _dbContext.Autors.Remove(autor);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorExists(int id)
        {
            return _dbContext.Autors.Any(a => a.IdAutor == id);
        }
    }
}
