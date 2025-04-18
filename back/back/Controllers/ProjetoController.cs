using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjetoController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public ProjetoController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    // GET: api/projeto
    [HttpGet]
    public ActionResult<IEnumerable<Projeto>> GetProjetos()
    {
        return Ok(_context.Projetos.ToList());
    }

    // GET: api/projeto/{id}
    [HttpGet("{id}")]
    public ActionResult<Projeto> GetProjeto(int id)
    {
        var projeto = _context.Projetos.Find(id);

        if (projeto == null)
            return NotFound();

        return Ok(projeto);
    }

    // POST: api/projeto
    [HttpPost]
    public ActionResult<Projeto> PostProjeto(ProjetoCreateDTO dto)
    {
        var novo = new Projeto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            ClienteId = dto.ClienteId,
            HorasTrabalho = dto.HorasTrabalho,
            UtilizadorId = dto.UtilizadorId,
            Estado = dto.Estado
        };

        _context.Projetos.Add(novo);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetProjeto), new { id = novo.Id }, novo);
    }

    // PUT: api/projeto/{id}
    [HttpPut("{id}")]
    public IActionResult PutProjeto(int id, Projeto projeto)
    {
        if (id != projeto.Id)
            return BadRequest();

        var projetoExistente = _context.Projetos.Find(id);
        if (projetoExistente == null)
            return NotFound();

        projetoExistente.Nome = projeto.Nome;
        projetoExistente.Descricao = projeto.Descricao;
        projetoExistente.DataInicio = projeto.DataInicio;
        projetoExistente.DataFim = projeto.DataFim;
        projetoExistente.ClienteId = projeto.ClienteId;
        projetoExistente.HorasTrabalho = projeto.HorasTrabalho;
        projetoExistente.UtilizadorId = projeto.UtilizadorId;
        projetoExistente.Estado = projeto.Estado;

        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/projeto/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteProjeto(int id)
    {
        var projeto = _context.Projetos.Find(id);
        if (projeto == null)
            return NotFound();

        _context.Projetos.Remove(projeto);
        _context.SaveChanges();

        return NoContent();
    }
}
