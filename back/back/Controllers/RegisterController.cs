using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public RegisterController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    // POST: api/register
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = HttpContext.User;

        // SEM login → só pode criar User (Regular)
        if (!(user.Identity?.IsAuthenticated ?? false))
        {
            if (dto.Tipo != "User") // Se não estiver autenticado, só pode criar tipo "User"
                return Unauthorized("Sem login, só é possível criar utilizadores do tipo 'User'.");
        }
        else
        {
            // UserManager pode criar qualquer tipo de utilizador (User, UserM, Admin)
            if (user.IsInRole("UserManager") || user.IsInRole("Admin"))
            {
                // Se for Admin ou UserManager, pode criar qualquer tipo
            }
            else
            {
                // Se for User, não pode criar ninguém
                return Unauthorized("Utilizadores do tipo 'User' não têm permissões para criar contas.");
            }
        }

        // Verificar se o email já existe
        var existe = _context.Utilizadores.Any(u => u.Email == dto.Email);
        if (existe)
            return Conflict("Já existe uma conta com este email.");

        var utilizador = new Utilizador
        {
            Nome = dto.Nome,
            Email = dto.Email,
            HorasDia = 8,
            Tipo = dto.Tipo, 
            IsAdmin = dto.Tipo == "Admin" 
        };

        var hasher = new PasswordHasher<Utilizador>();
        utilizador.Password = hasher.HashPassword(utilizador, dto.Password);

        _context.Utilizadores.Add(utilizador);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Conta criada com sucesso!",
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
