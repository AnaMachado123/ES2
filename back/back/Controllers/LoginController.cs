using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;
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
            // 1) Autenticação via serviço
            var token = loginSvc.Autenticar(dto);
            if (token == null)
                return Unauthorized("Credenciais inválidas.");

            // 2) Busca do utilizador
            var user = _context.Utilizadores.First(u => u.Email == dto.Email);

            // 3) Resposta correta para o frontend (token + dados no mesmo nível)
            return Ok(new
            {
                token,
                id = user.Id, // ✅ incluído para o frontend guardar na sessão
                nome = user.Nome,
                email = user.Email,
                tipo = user.Tipo == "UserM" ? "UserManager" : user.Tipo
            });
        }
    }
}
