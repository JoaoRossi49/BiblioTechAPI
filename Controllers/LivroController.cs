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
    [Route("api/livros")]
    public class LivrosController : ControllerBase
    {
        private readonly BibliotechContext _dbContext;

        public LivrosController(BibliotechContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> ObterLivros()
        {
            var livros = await _dbContext.Livros.ToListAsync();
            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> ObterLivro(int id)
        {
            var livro = await _dbContext.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            return Ok(livro);
        }

        [HttpPost]
        public async Task<ActionResult<Livro>> CriarLivro(Livro livro)
        {
            _dbContext.Livros.Add(livro);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterLivro), new { id = livro.IdLivo }, livro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLivro(int id, Livro livro)
        {
            if (id != livro.IdLivo)
            {
                return BadRequest();
            }

            _dbContext.Entry(livro).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
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
        public async Task<IActionResult> ExcluirLivro(int id)
        {
            var livro = await _dbContext.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _dbContext.Livros.Remove(livro);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool LivroExists(int id)
        {
            return _dbContext.Livros.Any(l => l.IdLivo == id);
        }
    }
}
