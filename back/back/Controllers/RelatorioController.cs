using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services; 
using BackendTesteESII.Models.Strategies;
using BackendTesteESII.Models.DTOs;  

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly IRelatorioService _service;

    public RelatorioController(IRelatorioService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Relatorio>> GetRelatorios()
        => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Relatorio> GetRelatorio(int id)
    {
        var r = _service.GetById(id);
        if (r == null) return NotFound();
        return Ok(r);
    }

    [HttpPost]
    public ActionResult<Relatorio> PostRelatorio(Relatorio relatorio)
    {
        var criado = _service.Create(relatorio);
        return CreatedAtAction(nameof(GetRelatorio), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult PutRelatorio(int id, Relatorio relatorio)
    {
        if (id != relatorio.Id) return BadRequest();
        if (!_service.Update(id, relatorio)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRelatorio(int id)
    {
        if (!_service.Delete(id)) return NotFound();
        return NoContent();
    }
    [HttpGet("mensal")]
    public IActionResult GetRelatorioMensal([FromQuery] int utilizadorId, [FromQuery] int mes, [FromQuery] int ano)
    {
        var strategy = new RelatorioMensalStrategy();
        var contextStrategy = new RelatorioContext<List<RelatorioMensalDTO>>(strategy);
        var resultado = contextStrategy.Executar(_service.GetContext(), utilizadorId, mes, ano); // ← ajusta para obter context

            return Ok(resultado);
    }
    [HttpGet("projeto")]
    public IActionResult GetRelatorioProjeto([FromQuery] int projetoId)
    {
        var strategy = new RelatorioProjetoStrategy();
        var contextStrategy = new RelatorioContextProjeto<List<RelatorioProjetoDTO>>(strategy);
        var resultado = contextStrategy.Executar(_service.GetContext(), projetoId);

        return Ok(resultado);
    }

}
