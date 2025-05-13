using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BackendTesteESII.Data; 

namespace BackendTesteESII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorService _service;
        private readonly GestaoServicosClientesContext _context;

        public UtilizadorController(IUtilizadorService service, GestaoServicosClientesContext context)
        {
            _service = service;
            _context = context;
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
            var user = HttpContext.User;
            var tipo = dto.Tipo?.Trim();

            if (string.IsNullOrWhiteSpace(tipo) ||
                (tipo != "User" && tipo != "UserManager" && tipo != "Admin"))
                return BadRequest("Tipo de utilizador inválido.");

            if (!(user.Identity?.IsAuthenticated ?? false))
            {
                if (tipo != "User")
                    return Forbid("Sem autenticação, só é permitido criar utilizadores do tipo 'User'.");
            }
            else
            {
                if (user.IsInRole("User"))
                    return Forbid("Utilizadores do tipo 'User' não podem criar novos utilizadores.");

                if (user.IsInRole("UserManager") && tipo == "Admin")
                    return Forbid("User Managers não podem criar Admins.");
            }

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

        [HttpPost("recuperar")]
        public IActionResult RecuperarPassword([FromBody] string email)
        {
            var sucesso = _service.RecuperarPassword(email);
            return sucesso ? Ok("Email enviado com sucesso.") : NotFound("Utilizador não encontrado.");
        }

       
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.Id.ToString() == userId);

            if (utilizador == null)
                return NotFound();

            var userInfo = new UserInfoDTO
            {
                Nome = utilizador.Nome,
                Email = utilizador.Email,
                Role = utilizador.Tipo
            };

            return Ok(userInfo);
        }
    }
}
