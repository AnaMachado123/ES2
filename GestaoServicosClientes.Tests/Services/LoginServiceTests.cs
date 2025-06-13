using NUnit.Framework;
using BackendTesteESII.Services;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GestaoServicosClientes.Tests.Services
{
    [TestFixture]
    public class LoginServiceTests
    {
        private GestaoServicosClientesContext _context;
        private LoginService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GestaoServicosClientesContext>()
                .UseInMemoryDatabase(databaseName: "LoginTestDb")
                .Options;

            _context = new GestaoServicosClientesContext(options);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "TestesUnitariosES2TrabalhoIndividualGestaoServicos!" },
                    { "Jwt:Issuer", "testissuer" },
                    { "Jwt:Audience", "testaudience" }
                })
                .Build();

            _service = new LoginService(_context, config);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [Test]
        public void Autenticar_CredenciaisValidas_DeveRetornarToken()
        {
            var hasher = new PasswordHasher<Utilizador>();
            var passwordHash = hasher.HashPassword(new Utilizador(), "SenhaTeste1");

            _context.Utilizadores.Add(new Utilizador
            {
                Email = "teste1@email.com",
                Password = passwordHash,
                Nome = "Teste1",
                Tipo = "User"
            });
            _context.SaveChanges();

            var request = new LoginRequest { Email = "teste1@email.com", Password = "SenhaTeste1" };

            var token = _service.Autenticar(request);

            Assert.That(token, Is.Not.Null);
            Assert.That(token.Length, Is.GreaterThan(10));
        }

        [Test]
        public void Autenticar_EmailInvalido_DeveRetornarNull()
        {
            var request = new LoginRequest { Email = "invalido@email.com", Password = "qualquer" };
            var result = _service.Autenticar(request);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Autenticar_PasswordInvalida_DeveRetornarNull()
        {
            var hasher = new PasswordHasher<Utilizador>();
            var passwordHash = hasher.HashPassword(new Utilizador(), "SenhaCorreta");

            _context.Utilizadores.Add(new Utilizador
            {
                Email = "teste2@email.com",
                Password = passwordHash,
                Nome = "Teste2",
                Tipo = "User"
            });
            _context.SaveChanges();

            var request = new LoginRequest { Email = "teste2@email.com", Password = "SenhaErrada" };
            var result = _service.Autenticar(request);

            Assert.That(result, Is.Null);
        }
    }
}
