using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

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
    public ActionResult<Utilizador> PostUtilizador(Utilizador utilizador)
    {
        _context.Utilizadores.Add(utilizador);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.Id }, utilizador);
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
}
