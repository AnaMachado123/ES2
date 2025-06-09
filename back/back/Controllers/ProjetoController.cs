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

            int userId = int.Parse(userIdStr);
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

            int userId = int.Parse(userIdStr);
            var novo = _service.Create(dto, userId);
            return CreatedAtAction(nameof(GetProjeto), new { id = novo.Id }, novo);
        }

        [HttpPut("{id}")]
        public IActionResult PutProjeto(int id, Projeto projeto)
        {
            if (id != projeto.Id) return BadRequest();

            return _service.Update(id, projeto) ? NoContent() : NotFound();
        }

        // ✅ ATUALIZADO: Apagar projeto com mensagem personalizada
        [HttpDelete("{id}")]
        public IActionResult DeleteProjeto(int id)
        {
            var projeto = _service.GetById(id);
            if (projeto == null)
                return NotFound("Projeto não encontrado.");

            var nome = projeto.Nome;

            var apagado = _service.Delete(id);
            if (!apagado)
                return BadRequest("Erro ao apagar o projeto.");

            return Ok(new { message = $"Projeto \"{nome}\" apagado com sucesso!" });
        }

        [HttpGet("{id}/detalhado")]
        public IActionResult GetDetalhado(int id)
        {
            var projeto = _service.GetDetalhadoById(id);
            return projeto == null ? NotFound() : Ok(projeto);
        }

        [HttpPut("{id}/concluir")]
        public IActionResult ConcluirProjeto(int id)
        {
            var sucesso = _service.ConcluirProjeto(id);
            return sucesso ? NoContent() : NotFound();
        }

        [HttpGet("{id}/valor")]
        public IActionResult GetValorTotalDoProjeto(int id)
        {
            var valor = _service.CalcularValorTotalProjeto(id);
            return Ok(new { ProjetoId = id, ValorTotal = valor });
        }

        [HttpGet("{id}/membros")]
        public IActionResult GetMembros(int id)
        {
            var membros = _service.GetMembrosDoProjeto(id);
            return Ok(membros);
        }
    }
}
