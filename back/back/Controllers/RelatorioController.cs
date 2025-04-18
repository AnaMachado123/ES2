using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;   // <— nova referência

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
}
