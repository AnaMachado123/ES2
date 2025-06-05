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
        public IActionResult GetProjetos()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null)
                return Unauthorized();

            int userId = int.Parse(userIdStr); // ← o UtilizadorId é inteiro, dí a conversão

            return Ok(_service.GetByUserId(userId));
        }

        [HttpGet("{id}")]
        public IActionResult GetProjeto(int id)
        {
            var projeto = _service.GetDetalhadoById(id);
            return projeto == null ? NotFound() : Ok(projeto);
        }

        [HttpPost]
        public IActionResult PostProjeto([FromBody] ProjetoCreateDTO dto)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null) return Unauthorized();

            int userId = int.Parse(userIdStr); // UtilizadorId como int

            var novo = _service.Create(dto, userId);
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
        
        [HttpGet("{id}/detalhado")]
        public IActionResult GetDetalhado(int id)
        {
            var projeto = _service.GetDetalhadoById(id);
            return projeto == null ? NotFound() : Ok(projeto);
        }

    }
}
