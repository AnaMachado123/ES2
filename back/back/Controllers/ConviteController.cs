using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConviteController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public ConviteController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Convite>> GetConvites()
    {
        return Ok(_context.Convites.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Convite> GetConvite(int id)
    {
        var convite = _context.Convites.Find(id);
        if (convite == null)
            return NotFound();

        return Ok(convite);
    }

    [HttpPost]
    public ActionResult<Convite> PostConvite(Convite convite)
    {
        _context.Convites.Add(convite);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetConvite), new { id = convite.Id }, convite);
    }

    [HttpPut("{id}")]
    public IActionResult PutConvite(int id, Convite convite)
    {
        if (id != convite.Id)
            return BadRequest();

        var existente = _context.Convites.Find(id);
        if (existente == null)
            return NotFound();

        existente.UtilizadorId = convite.UtilizadorId;
        existente.ProjetoId = convite.ProjetoId;
        existente.Estado = convite.Estado;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteConvite(int id)
    {
        var convite = _context.Convites.Find(id);
        if (convite == null)
            return NotFound();

        _context.Convites.Remove(convite);
        _context.SaveChanges();
        return NoContent();
    }
}
