using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTesteESII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public RegisterController(GestaoServicosClientesContext context)
        {
            _context = context;
        }

  
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

       
            if (dto.Tipo is null) dto.Tipo = string.Empty;

            dto.Tipo = dto.Tipo.Trim();      
            dto.Tipo = dto.Tipo switch
            {
                "UserM"      or "manager" => "UserManager",
                "regular"                => "User",
                "admin"                  => "Admin",       
                _                        => dto.Tipo       
            };

            
            var tiposValidos = new[] { "User", "UserManager", "Admin" };
            if (!tiposValidos.Contains(dto.Tipo))
                return BadRequest("Tipo de utilizador inválido.");

          
            var user = HttpContext.User;

       
            if (!(user.Identity?.IsAuthenticated ?? false))
            {
                if (dto.Tipo != "User")
                    return Unauthorized("Sem login, só é possível criar utilizadores do tipo 'User'.");
            }
            else
            {
              
                if (user.IsInRole("User"))
                    return Unauthorized("Utilizadores do tipo 'User' não têm permissões para criar contas.");

                if (user.IsInRole("UserManager") && dto.Tipo == "Admin")
                    return Unauthorized("User Managers não podem criar Admins.");
            }

            if (_context.Utilizadores.Any(u => u.Email == dto.Email))
                return Conflict("Já existe uma conta com este email.");

           
            var utilizador = new Utilizador
            {
                Nome     = dto.Nome,
                Email    = dto.Email,
                HorasDia = 8,
                Tipo     = dto.Tipo,
                IsAdmin  = dto.Tipo == "Admin"
            };

            var hasher   = new PasswordHasher<Utilizador>();
            utilizador.Password = hasher.HashPassword(utilizador, dto.Password);

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

    
            return Ok(new
            {
                mensagem   = "Conta criada com sucesso!",
                utilizador = new
                {
                    utilizador.Id,
                    utilizador.Nome,
                    utilizador.Email,
                    tipo = utilizador.Tipo       
                }
            });
        }
    }
}
