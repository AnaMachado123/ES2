using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public UtilizadorController(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        // GET: api/utilizador
        [HttpGet]
        public ActionResult<IEnumerable<Utilizador>> GetUtilizadores()
        {
            return Ok(_context.Utilizadores.ToList());
        }

        // GET: api/utilizador/5
        [HttpGet("{id}")]
        public ActionResult<Utilizador> GetUtilizador(int id)
        {
            var utilizador = _context.Utilizadores.Find(id);
            if (utilizador == null)
                return NotFound();

            return Ok(utilizador);
        }

        // POST: api/utilizador
        [HttpPost]
        public ActionResult<Utilizador> PostUtilizador(UtilizadorCreateDTO dto)
        {
            // Cria o utilizador base com os dados do DTO
            var novo = new Utilizador
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Password = dto.Password,
                HorasDia = 8,
                IsAdmin = dto.Tipo.Equals("admin", StringComparison.OrdinalIgnoreCase)
            };

            _context.Utilizadores.Add(novo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUtilizador), new { id = novo.Id }, novo);
        }

        // PUT: api/utilizador/5
        [HttpPut("{id}")]
        public IActionResult PutUtilizador(int id, Utilizador utilizador)
        {
            if (id != utilizador.Id)
                return BadRequest();

            var u = _context.Utilizadores.Find(id);
            if (u == null)
                return NotFound();

            u.Nome = utilizador.Nome;
            u.Email = utilizador.Email;
            u.Password = utilizador.Password;
            u.HorasDia = utilizador.HorasDia;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/utilizador/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUtilizador(int id)
        {
            var utilizador = _context.Utilizadores.Find(id);
            if (utilizador == null)
                return NotFound();

            _context.Utilizadores.Remove(utilizador);
            _context.SaveChanges();
            return NoContent();
        }

        // GET: api/utilizador/verificar-permissao/5
        [HttpGet("verificar-permissao/{id}")]
        public IActionResult VerificarPermissao(int id)
        {
            var userFromDb = _context.Utilizadores.FirstOrDefault(u => u.Id == id);
            if (userFromDb == null)
                return NotFound("Utilizador não encontrado.");

            // Usa a Factory com base no IsAdmin
            var utilizador = UtilizadorFactory.Criar(userFromDb);

            if (utilizador.PodeGerirUtilizadores())
                return Ok("Este utilizador tem permissão para realizar a gestão de outros utilizadores.");
            else
                return Ok("Este utilizador NÃO tem permissão para  realizar a gestão de outros utilizadores.");
        }
    }
}
