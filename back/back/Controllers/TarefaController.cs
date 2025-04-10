using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public TarefaController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    // GET: api/tarefa
    [HttpGet]
    public ActionResult<IEnumerable<Tarefa>> GetTarefas()
    {
        return Ok(_context.Tarefas.ToList());
    }

    // GET: api/tarefa/{id}
    [HttpGet("{id}")]
    public ActionResult<Tarefa> GetTarefa(int id)
    {
        var tarefa = _context.Tarefas.Find(id);
        if (tarefa == null)
            return NotFound();

        return Ok(tarefa);
    }

    // POST: api/tarefa
    [HttpPost]
    public ActionResult<Tarefa> PostTarefa(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
    }

    // PUT: api/tarefa/{id}
    [HttpPut("{id}")]
    public IActionResult PutTarefa(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id)
            return BadRequest();

        var t = _context.Tarefas.Find(id);
        if (t == null)
            return NotFound();

        t.Descricao = tarefa.Descricao;
        t.DataInicio = tarefa.DataInicio;
        t.DataFim = tarefa.DataFim;
        t.Status = tarefa.Status;
        t.ProjetoId = tarefa.ProjetoId;
        t.UtilizadorId = tarefa.UtilizadorId;
        t.HorasGastas = tarefa.HorasGastas;

        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/tarefa/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTarefa(int id)
    {
        var tarefa = _context.Tarefas.Find(id);
        if (tarefa == null)
            return NotFound();

        _context.Tarefas.Remove(tarefa);
        _context.SaveChanges();
        return NoContent();
    }
}
