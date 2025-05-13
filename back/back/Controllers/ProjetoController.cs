using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Services;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Controllers
{
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
        [HttpGet]
public IActionResult GetProjetos()
{
    return Ok(_service.GetAll()); // ✅ agora devolve nome do cliente
}

        [HttpGet("{id}")]
        public IActionResult GetProjeto(int id)
        {
            var projeto = _service.GetById(id);
            return projeto == null ? NotFound() : Ok(projeto);
        }

        [HttpPost]
        public IActionResult PostProjeto([FromBody] ProjetoCreateDTO dto)
        {
            var novo = _service.Create(dto);
            return CreatedAtAction(nameof(GetProjeto), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}")]
        public IActionResult PutProjeto(int id, Projeto projeto)
        {
            if (id != projeto.Id) return BadRequest();

            return _service.Update(id, projeto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProjeto(int id)
        {
            return _service.Delete(id) ? NoContent() : NotFound();
        }
    }
}
