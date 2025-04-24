using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services; // 👈 Importa o namespace do serviço

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjetoController : ControllerBase
{
    private readonly IProjetoService _service;

    public ProjetoController(IProjetoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetProjetos() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetProjeto(int id)
    {
        var projeto = _service.GetById(id);
        return projeto == null ? NotFound() : Ok(projeto);
    }

    [HttpPost]
    public IActionResult PostProjeto(ProjetoCreateDTO dto)
    {
        var criado = _service.Create(dto);
        return CreatedAtAction(nameof(GetProjeto), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult PutProjeto(int id, Projeto projeto)
    {
        if (id != projeto.Id) return BadRequest();
        if (!_service.Update(id, projeto)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProjeto(int id)
    {
        if (!_service.Delete(id)) return NotFound();
        return NoContent();
    }
}
