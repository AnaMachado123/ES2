using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;          // ← garante que tens este using
using System.Linq;

namespace BackendTesteESII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public LoginController(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        // POST: api/Login
        [HttpPost]
        public IActionResult Login(
            [FromBody] LoginRequest dto,
            [FromServices] ILoginService loginSvc)
        {
            /* 1) Autentica-se através do serviço */
            var token = loginSvc.Autenticar(dto);
            if (token == null)
                return Unauthorized("Credenciais inválidas.");

            /* 2) Busca o utilizador só para devolver info ao front-end */
            var user = _context.Utilizadores.First(u => u.Email == dto.Email);

            /* 3) Resposta final */
            return Ok(new
            {
                token,
                utilizador = new
                {
                    user.Id,
                    user.Nome,
                    user.Email,
                    // normaliza caso ainda exista "UserM" guardado na BD
                    tipo = user.Tipo == "UserM" ? "UserManager" : user.Tipo
                }
            });
        }
    }
}
