using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BackendTesteESII.Services
{
    public class LoginService : ILoginService
    {
        private readonly GestaoServicosClientesContext _context;
        private readonly IConfiguration _config;

        public LoginService(GestaoServicosClientesContext context, IConfiguration config)
        {
            _context = context;
            _config  = config;
        }

        /// <summary>
        /// Devolve uma string JWT ou null se as credenciais forem inválidas
        /// </summary>
        public string? Autenticar(LoginRequest request)
        {
            var utilizador = _context.Utilizadores
                                     .FirstOrDefault(u => u.Email == request.Email);

            if (utilizador == null) return null;

            var hasher   = new PasswordHasher<Utilizador>();
            var verifRes = hasher.VerifyHashedPassword(utilizador,
                                                       utilizador.Password,
                                                       request.Password);

            if (verifRes == PasswordVerificationResult.Failed)
                return null;

            // -----------------------
            //  NORMALIZAÇÃO DA ROLE
            // -----------------------
            string role = utilizador.Tipo switch
            {
                // Se ainda existir "UserM" na BD, converte para o nome oficial
                "UserM"        => "UserManager",
                "Admin" or "UserManager" or "User" => utilizador.Tipo,
                _              => "User"  // fallback defensivo
            };

            // -----------------------
            //  CLAIMS
            // -----------------------
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, utilizador.Id.ToString()),
                new Claim(ClaimTypes.Name,          utilizador.Nome),
                new Claim(ClaimTypes.Email,         utilizador.Email),
                new Claim(ClaimTypes.Role,          role)          // <- chave do problema
            };

            // -----------------------
            //  TOKEN JWT
            // -----------------------
            var key   = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:  _config["Jwt:Issuer"],
                audience:_config["Jwt:Audience"],
                claims:  claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
