
using NUnit.Framework;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using BackendTesteESII.Services;
using BackendTesteESII.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace BackendTesteESII.Tests.Services
{
    [TestFixture]
    public class ProjetoServiceTests
    {
        private ProjetoService _projetoService;
        private GestaoServicosClientesContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GestaoServicosClientesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new GestaoServicosClientesContext(options);
            _context.Database.EnsureCreated();
            _projetoService = new ProjetoService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void CriarProjeto_ComDadosValidos_DeveCriarProjeto()
        {
            var dto = new ProjetoCreateDTO
            {
                Nome = "Teste",
                Descricao = "Descrição",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(2),
                ClienteId = 1,
                UtilizadorId = 1,
                Estado = "Planeado",
                HorasTrabalho = 10
            };

            var result = _projetoService.Create(dto, 1);
            result.Nome.Should().Be("Teste");
        }

        [Test]
        public void CriarProjeto_ComDataFimAntesDoInicio_DeveSalvarMesmoAssim()
        {
            var dto = new ProjetoCreateDTO
            {
                Nome = "Teste Data",
                Descricao = "Erro de data",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(-1),
                ClienteId = 1,
                UtilizadorId = 1,
                Estado = "Pendente",
                HorasTrabalho = 20
            };

            var result = _projetoService.Create(dto, 1);
            result.Should().NotBeNull();
        }

        [Test]
        public void GetByUserId_DeveRetornarProjetosCriadosOuPartilhados()
        {
            _context.Projetos.Add(new Projeto { Id = 1, Nome = "P1", UtilizadorId = 42, ClienteId = 1, Estado = "Novo", HorasTrabalho = 10, DataInicio = DateTime.Now, DataFim = DateTime.Now });
            _context.UtilizadorProjetos.Add(new UtilizadorProjeto { ProjetoId = 2, UtilizadorId = 42, Projeto = new Projeto { Id = 2, Nome = "P2", UtilizadorId = 99, ClienteId = 2, Estado = "Em curso", HorasTrabalho = 5, DataInicio = DateTime.Now, DataFim = DateTime.Now } });
            _context.Clientes.AddRange(new Cliente { Id = 1, Nome = "Cliente1" }, new Cliente { Id = 2, Nome = "Cliente2" });
            _context.SaveChanges();

            var lista = _projetoService.GetByUserId(42).ToList();
            lista.Should().HaveCount(2);
        }

        [Test]
        public void Delete_ProjetoExistente_DeveApagar()
        {
            var projeto = new Projeto { Id = 1, Nome = "Projeto X", ClienteId = 1, UtilizadorId = 1, Estado = "Ativo", HorasTrabalho = 15, DataInicio = DateTime.Now, DataFim = DateTime.Now };
            _context.Projetos.Add(projeto);
            _context.SaveChanges();

            var result = _projetoService.Delete(1);
            result.Should().BeTrue();
            _context.Projetos.Find(1).Should().BeNull();
        }

        [Test]
        public void Delete_ProjetoInexistente_DeveRetornarFalse()
        {
            var result = _projetoService.Delete(123);
            result.Should().BeFalse();
        }

        [Test]
        public void GetDetalhadoById_DeveRetornarDadosCompletos()
        {
            var projeto = new Projeto { Id = 1, Nome = "Detalhado", ClienteId = 1, UtilizadorId = 2, Estado = "Concluído", HorasTrabalho = 8, DataInicio = DateTime.Now, DataFim = DateTime.Now };
            var cliente = new Cliente { Id = 1, Nome = "Cliente1" };
            var utilizador = new Utilizador { Id = 2, Nome = "User" };

            _context.Projetos.Add(projeto);
            _context.Clientes.Add(cliente);
            _context.Utilizadores.Add(utilizador);
            _context.SaveChanges();

            var result = _projetoService.GetDetalhadoById(1);
            result.Should().NotBeNull();
            result.Nome.Should().Be("Detalhado");
            result.NomeCliente.Should().Be("Cliente1");
            result.NomeCriador.Should().Be("User");
        }
    }
}
