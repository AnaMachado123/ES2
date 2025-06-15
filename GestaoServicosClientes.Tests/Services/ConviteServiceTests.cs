using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;
using System;


namespace TestesUnitarios
{
    public class ConviteServiceTests
    {
        private GestaoServicosClientesContext _context;
        private ConviteService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GestaoServicosClientesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // base nova por teste
                .Options;

            _context = new GestaoServicosClientesContext(options);
            _service = new ConviteService(_context);

            // Dados simulados
            _context.Projetos.Add(new Projeto
            {
                Id = 1,
                UtilizadorId = 100
            });
            _context.UtilizadorProjetos.Add(new UtilizadorProjeto
            {
                ProjetoId = 1,
                UtilizadorId = 200
            });
            _context.SaveChanges();
        }

        [Test]
        public void NaoDeveCriarConviteSeUtilizadorJaPertencerAoProjeto()
        {
            var dto = new ConviteCreateDTO
            {
                ProjetoId = 1,
                UtilizadorId = 200 // já está no projeto
            };

            var ex = Assert.Throws<Exception>(() =>
                _service.CreateComValidacao(userIdLogado: 100, dto));

            Assert.That(ex!.Message, Is.EqualTo("Utilizador já está no projeto."));
        }


        [Test]
        public void AceitarConvite_DeveAceitarEAssociarUtilizador()
        {
            int conviteId = 1;
            int utilizadorId = 200;
            int projetoId = 2;

            _context.Projetos.Add(new Projeto { Id = projetoId, UtilizadorId = 201 });
            _context.Convites.Add(new Convite
            {
                Id = conviteId,
                UtilizadorId = utilizadorId,
                ProjetoId = projetoId,
                Estado = "Pendente"
            });

            _context.SaveChanges();


            var sucesso = _service.AceitarConvite(conviteId);
            Assert.That(sucesso, Is.True);

            var convite = _context.Convites.Find(conviteId);
            Assert.That(convite!.Estado, Is.EqualTo("Aceite"));

            var associacao = _context.UtilizadorProjetos
                .FirstOrDefault(up => up.UtilizadorId == utilizadorId && up.ProjetoId == projetoId);

            Assert.That(associacao, Is.Not.Null);
        }

        [Test]
        public void RecusarConvite_DeveAtualizarEstadoParaRecusado()
        {
            int conviteId = 20;

            _context.Convites.Add(new Convite
            {
                Id = conviteId,
                UtilizadorId = 500,
                ProjetoId = 50,
                Estado = "Pendente"
            });
            _context.SaveChanges();

            var sucesso = _service.RecusarConvite(conviteId);

            Assert.That(sucesso, Is.True);

            var convite = _context.Convites.Find(conviteId);
            Assert.That(convite!.Estado, Is.EqualTo("Recusado"));
        }

        [Test]
        public void AceitarConvite_DeveFalharSeConviteNaoEstiverPendente()
        {
            int conviteId = 30;

            _context.Convites.Add(new Convite
            {
                Id = conviteId,
                UtilizadorId = 600,
                ProjetoId = 60,
                Estado = "Aceite" // já foi aceite
            });
            _context.SaveChanges();

            var sucesso = _service.AceitarConvite(conviteId);

            Assert.That(sucesso, Is.False);
        }
        

        [Test]
        public void GetDTOsByUtilizador_DeveRetornarConvitesDoUtilizador()
        {
            int utilizadorId = 700;
            int projetoId = 70;

            _context.Projetos.Add(new Projeto
            {
                Id = projetoId,
                Nome = "Projeto Teste",
                UtilizadorId = 999
            });

            _context.Convites.Add(new Convite
            {
                Id = 40,
                ProjetoId = projetoId,
                UtilizadorId = utilizadorId,
                Estado = "Pendente"
            });

            _context.SaveChanges();

            var convites = _service.GetDTOsByUtilizador(utilizadorId).ToList();

            Assert.That(convites, Has.Count.EqualTo(1));
            Assert.That(convites[0].ProjetoId, Is.EqualTo(projetoId));
            Assert.That(convites[0].ProjetoNome, Is.EqualTo("Projeto Teste"));
            Assert.That(convites[0].Estado, Is.EqualTo("Pendente"));
        }


        

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

    }

}
