using BackendTesteESII.Data;
using BackendTesteESII.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendTesteESII.Services
{
    public class LoginService : ILoginService
    {
        private readonly GestaoServicosClientesContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(GestaoServicosClientesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string? Autenticar(LoginRequest request)
        {
            var utilizador = _context.Utilizadores.FirstOrDefault(u =>
                u.Email == request.Email && u.Password == request.Password);

            if (utilizador == null) return null;

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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
