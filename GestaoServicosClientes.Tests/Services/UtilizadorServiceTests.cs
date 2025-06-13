using NUnit.Framework;
using Moq;
using BackendTesteESII.Services;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestaoServicosClientes.Tests.Services
{
    [TestFixture]
    public class UtilizadorServiceTests
    {
        private GestaoServicosClientesContext _context;
        private UtilizadorService _service;
        private Mock<IEmailService> _emailMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GestaoServicosClientesContext>()
                .UseInMemoryDatabase(databaseName: "UtilizadorTestDb")
                .Options;

            _context = new GestaoServicosClientesContext(options);
            _emailMock = new Mock<IEmailService>();

            _service = new UtilizadorService(_context, _emailMock.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [Test]
        public void Create_DeveCriarUtilizador()
        {
            var dto = new UtilizadorCreateDTO
            {
                Nome = "Teste1",
                Email = "teste1@email.com",
                Password = "SenhaTeste1",
                Tipo = "User"
            };

            var novo = _service.Create(dto);

            Assert.That(novo, Is.Not.Null);
            Assert.That(novo.Email, Is.EqualTo("teste1@email.com"));
            Assert.That(_context.Utilizadores.Any(u => u.Email == "teste1@email.com"), Is.True);
        }

        [Test]
        public void RecuperarPassword_EmailValido_DeveEnviarEmail()
        {
            var utilizador = new Utilizador
            {
                Nome = "Teste1",
                Email = "teste1@email.com",
                Password = "SenhaAntiga",
                Tipo = "User"
            };
            _context.Utilizadores.Add(utilizador);
            _context.SaveChanges();

            var resultado = _service.RecuperarPassword("teste1@email.com");

            Assert.That(resultado, Is.True);
            _emailMock.Verify(e => e.EnviarEmail("teste1@email.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RecuperarPassword_EmailInvalido_DeveRetornarFalse()
        {
            var resultado = _service.RecuperarPassword("invalido@email.com");

            Assert.That(resultado, Is.False);
            _emailMock.Verify(e => e.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
