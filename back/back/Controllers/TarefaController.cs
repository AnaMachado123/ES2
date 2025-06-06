using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;
using Microsoft.EntityFrameworkCore;
using BackendTesteESII.Data;



namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : ControllerBase

{

    private readonly GestaoServicosClientesContext _context;
    private readonly ITarefaService _service;

    public TarefaController(GestaoServicosClientesContext context, ITarefaService service)
    {
        _context = context;
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
    [HttpGet("emcurso")]
    public IActionResult GetTarefasEmCurso([FromQuery] int utilizadorId)
    {
        var tarefas = _context.Tarefas
            .Where(t => t.UtilizadorId == utilizadorId && t.Status.ToLower() != "finalizada")
            .ToList();

        return Ok(tarefas);
    }
    [HttpPut("{id}/mover")]
    public IActionResult MoverTarefaParaOutroProjeto(int id, [FromQuery] int novoProjetoId)
    {
        var tarefa = _service.GetById(id);
        if (tarefa == null) return NotFound("Tarefa não encontrada.");

        tarefa.ProjetoId = novoProjetoId;

        if (!_service.Update(id, tarefa)) return BadRequest("Erro ao mover tarefa.");

        return Ok("Tarefa movida com sucesso para o novo projeto.");
    }
    [HttpPut("{id}/finalizar")]
    public IActionResult FinalizarTarefa(int id)
    {
        var tarefa = _service.GetById(id);
        if (tarefa == null) return NotFound("Tarefa não encontrada.");

        tarefa.Status = "finalizada";
        tarefa.DataFim = DateTime.UtcNow; // ou manter como está se quiseres

        if (!_service.Update(id, tarefa)) return BadRequest("Erro ao finalizar tarefa.");

        return Ok("Tarefa finalizada com sucesso.");
    }
    
    [HttpGet("finalizadas")]
    public IActionResult GetTarefasFinalizadas([FromQuery] int utilizadorId)
    {
        var tarefas = _context.Tarefas
            .Where(t => t.UtilizadorId == utilizadorId && t.Status.ToLower() == "finalizada")
            .ToList();

        return Ok(tarefas);
    }


}
