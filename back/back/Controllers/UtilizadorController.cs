using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back.Models;

namespace back.Controllers
{

    public class UtilizadorRequest {
        public string Nome { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
    }

    [Route("api/utilizador")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public UtilizadorController(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        // GET: api/Utilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadors()
        {
            return await _context.Utilizadors.ToListAsync();
        }

        // GET: api/Utilizador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return utilizador;
        }

        // POST: api/Utilizador/adicionar
        [HttpPost("adicionar")]
        public async Task<ActionResult<Utilizador>> PostUtilizador(UtilizadorRequest utilizador)
        {
            // Map the properties from the UtilizadorRequest to the new Utilizador entity
            Utilizador utilizadorVdd = new Utilizador
            {
                Nome = utilizador.Nome,
                Password = utilizador.Password,
                Email = utilizador.Email,
            };

            _context.Utilizadors.Add(utilizadorVdd);
            await _context.SaveChangesAsync();

            // Return the newly created entity using its generated Id
            return CreatedAtAction(nameof(GetUtilizador), new { id = utilizadorVdd.Id }, utilizadorVdd);
        }


        // PUT: api/Utilizador/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, Utilizador utilizador)
        {
            if (id != utilizador.Id)
            {
                return BadRequest();
            }

            _context.Entry(utilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadorExists(id))
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

        // DELETE: api/Utilizador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizador(int id)
        {
            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizadors.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadors.Any(e => e.Id == id);
        }
    }
}
