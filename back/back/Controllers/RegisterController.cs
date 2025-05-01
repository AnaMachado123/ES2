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

        // POST: api/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            /* ────────────────────────────────────────────────────────────────
               1. NORMALIZAR ALIASES
               ───────────────────────────────────────────────────────────── */
            if (dto.Tipo is null) dto.Tipo = string.Empty;

            dto.Tipo = dto.Tipo.Trim();        // tira espaços
            dto.Tipo = dto.Tipo switch
            {
                "UserM"      or "manager" => "UserManager",
                "regular"                => "User",
                "admin"                  => "Admin",       // já coincide, mas garante case
                _                        => dto.Tipo       // mantém como veio
            };

            /* ────────────────────────────────────────────────────────────────
               2. VALIDAÇÃO DE TIPO OFICIAL
               ───────────────────────────────────────────────────────────── */
            var tiposValidos = new[] { "User", "UserManager", "Admin" };
            if (!tiposValidos.Contains(dto.Tipo))
                return BadRequest("Tipo de utilizador inválido.");

            /* ────────────────────────────────────────────────────────────────
               3. REGRAS DE PERMISSÃO
               ───────────────────────────────────────────────────────────── */
            var user = HttpContext.User;

            // Sem login → só pode criar User
            if (!(user.Identity?.IsAuthenticated ?? false))
            {
                if (dto.Tipo != "User")
                    return Unauthorized("Sem login, só é possível criar utilizadores do tipo 'User'.");
            }
            else
            {
                // User não pode criar ninguém
                if (user.IsInRole("User"))
                    return Unauthorized("Utilizadores do tipo 'User' não têm permissões para criar contas.");

                // UserManager não pode criar Admin
                if (user.IsInRole("UserManager") && dto.Tipo == "Admin")
                    return Unauthorized("User Managers não podem criar Admins.");
            }

            /* ────────────────────────────────────────────────────────────────
               4. VERIFICAÇÃO DE EMAIL
               ───────────────────────────────────────────────────────────── */
            if (_context.Utilizadores.Any(u => u.Email == dto.Email))
                return Conflict("Já existe uma conta com este email.");

            /* ────────────────────────────────────────────────────────────────
               5. CRIA UTILIZADOR
               ───────────────────────────────────────────────────────────── */
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

            /* ────────────────────────────────────────────────────────────────
               6. RESPOSTA
               ───────────────────────────────────────────────────────────── */
            return Ok(new
            {
                mensagem   = "Conta criada com sucesso!",
                utilizador = new
                {
                    utilizador.Id,
                    utilizador.Nome,
                    utilizador.Email,
                    tipo = utilizador.Tipo          // “User”, “UserManager” ou “Admin”
                }
            });
        }
    }
}
