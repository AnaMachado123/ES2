using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public RelatorioController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Relatorio>> GetRelatorios()
    {
        return Ok(_context.Relatorios.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Relatorio> GetRelatorio(int id)
    {
        var relatorio = _context.Relatorios.Find(id);
        if (relatorio == null)
            return NotFound();

        return Ok(relatorio);
    }

    [HttpPost]
    public ActionResult<Relatorio> PostRelatorio(Relatorio relatorio)
    {
        _context.Relatorios.Add(relatorio);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetRelatorio), new { id = relatorio.Id }, relatorio);
    }

    [HttpPut("{id}")]
    public IActionResult PutRelatorio(int id, Relatorio relatorio)
    {
        if (id != relatorio.Id)
            return BadRequest();

        var existente = _context.Relatorios.Find(id);
        if (existente == null)
            return NotFound();

        existente.UtilizadorId = relatorio.UtilizadorId;
        existente.Mes = relatorio.Mes;
        existente.Ano = relatorio.Ano;
        existente.TotalHoras = relatorio.TotalHoras;
        existente.TotalPreco = relatorio.TotalPreco;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRelatorio(int id)
    {
        var relatorio = _context.Relatorios.Find(id);
        if (relatorio == null)
            return NotFound();

        _context.Relatorios.Remove(relatorio);
        _context.SaveChanges();
        return NoContent();
    }
}