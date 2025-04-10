using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioProjetoController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public RelatorioProjetoController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RelatorioProjeto>> GetRelatoriosProjeto()
    {
        return Ok(_context.RelatoriosProjeto.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<RelatorioProjeto> GetRelatorioProjeto(int id)
    {
        var relatorio = _context.RelatoriosProjeto.Find(id);
        if (relatorio == null)
            return NotFound();

        return Ok(relatorio);
    }

    [HttpPost]
    public ActionResult<RelatorioProjeto> PostRelatorioProjeto(RelatorioProjeto relatorio)
    {
        _context.RelatoriosProjeto.Add(relatorio);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetRelatorioProjeto), new { id = relatorio.Id }, relatorio);
    }

    [HttpPut("{id}")]
    public IActionResult PutRelatorioProjeto(int id, RelatorioProjeto relatorio)
    {
        if (id != relatorio.Id)
            return BadRequest();

        var existente = _context.RelatoriosProjeto.Find(id);
        if (existente == null)
            return NotFound();

        existente.ProjetoId = relatorio.ProjetoId;
        existente.ClienteId = relatorio.ClienteId;
        existente.TotalHoras = relatorio.TotalHoras;
        existente.TotalPreco = relatorio.TotalPreco;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRelatorioProjeto(int id)
    {
        var relatorio = _context.RelatoriosProjeto.Find(id);
        if (relatorio == null)
            return NotFound();

        _context.RelatoriosProjeto.Remove(relatorio);
        _context.SaveChanges();
        return NoContent();
    }
}
