using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;
    private readonly IConfiguration _configuration;

    public LoginController(GestaoServicosClientesContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var utilizador = _context.Utilizadores.FirstOrDefault(u =>
            u.Email == request.Email && u.Password == request.Password);

        if (utilizador == null)
            return Unauthorized("Credenciais inválidas.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utilizador.Id.ToString()),
            new Claim(ClaimTypes.Name, utilizador.Nome),
            new Claim(ClaimTypes.Email, utilizador.Email),
            new Claim(ClaimTypes.Role, utilizador.IsAdmin ? "Admin" : "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
}
