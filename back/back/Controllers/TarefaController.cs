using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly ITarefaService _service;

    public TarefaController(ITarefaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetTarefas() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetTarefa(int id)
    {
        var tarefa = _service.GetById(id);
        return tarefa == null ? NotFound() : Ok(tarefa);
    }

    [HttpPost]
    public IActionResult PostTarefa(Tarefa tarefa)
    {
        var criada = _service.Create(tarefa);
        return CreatedAtAction(nameof(GetTarefa), new { id = criada.Id }, criada);
    }

    [HttpPut("{id}")]
    public IActionResult PutTarefa(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id) return BadRequest();
        if (!_service.Update(id, tarefa)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTarefa(int id)
    {
        if (!_service.Delete(id)) return NotFound();
        return NoContent();
    }
}
