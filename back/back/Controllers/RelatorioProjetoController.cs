using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioProjetoController : ControllerBase
{
    private readonly IRelatorioProjetoService _service;

    public RelatorioProjetoController(IRelatorioProjetoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetRelatorios() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetRelatorio(int id)
    {
        var relatorio = _service.GetById(id);
        return relatorio == null ? NotFound() : Ok(relatorio);
    }

    [HttpPost]
    public IActionResult PostRelatorio(RelatorioProjeto relatorio)
    {
        var criado = _service.Create(relatorio);
        return CreatedAtAction(nameof(GetRelatorio), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult PutRelatorio(int id, RelatorioProjeto relatorio)
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
}
