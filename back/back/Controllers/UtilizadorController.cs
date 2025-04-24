using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorService _service;

        public UtilizadorController(IUtilizadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetUtilizadores() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetUtilizador(int id)
        {
            var utilizador = _service.GetById(id);
            return utilizador == null ? NotFound() : Ok(utilizador);
        }

        [HttpPost]
        public IActionResult PostUtilizador(UtilizadorCreateDTO dto)
        {
            var novo = _service.Create(dto);
            return CreatedAtAction(nameof(GetUtilizador), new { id = novo.Id }, novo);
        }


        [HttpPut("{id}")]
        public IActionResult PutUtilizador(int id, Utilizador utilizador)
        {
            if (id != utilizador.Id)
                return BadRequest();

            if (!_service.Update(id, utilizador))
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUtilizador(int id)
        {
            if (!_service.Delete(id))
                return NotFound();

            return NoContent();
        }

        [HttpGet("verificar-permissao/{id}")]
        public IActionResult VerificarPermissao(int id)
        {
            var mensagem = _service.VerificarPermissao(id);
            return mensagem == null
                ? NotFound("Utilizador não encontrado.")
                : Ok(mensagem);
        }
    }
}
