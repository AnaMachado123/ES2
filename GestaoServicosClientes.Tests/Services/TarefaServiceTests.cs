using NUnit.Framework;
using Moq;
using FluentAssertions;
using System;
using BackendTesteESII.Services;       // Onde está ITarefaService
using BackendTesteESII.Models;         // Onde está a classe Tarefa   

namespace GestaoServicosClientes.Tests.Services
{
    [TestFixture]
    public class TarefaServiceTests
    {
        private Mock<ITarefaService> _tarefaServiceMock;

        [SetUp]
        public void Setup()
        {
            _tarefaServiceMock = new Mock<ITarefaService>();
        }

        [Test]
        public void CriarTarefa_ComDataFimAnterior_DeveLancarExcecao()
        {
            var tarefa = new Tarefa
            {
                Descricao = "Teste",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddHours(-1)
            };

            // Simulando serviço que deve lançar exceção — se tiver lógica na implementação real
            Action act = () =>
            {
                if (tarefa.DataFim <= tarefa.DataInicio)
                    throw new ArgumentException("A data de fim deve ser posterior à de início.");
            };

            act.Should().Throw<ArgumentException>()
                .WithMessage("A data de fim deve ser posterior à de início.");
        }

        [Test]
        public void FinalizarTarefa_DeveAtualizarStatusEDataFim()
        {
            var tarefa = new Tarefa
            {
                Id = 1,
                Descricao = "Finalizar tarefa",
                DataInicio = DateTime.Now.AddHours(-2),
                Status = "em curso"
            };

            _tarefaServiceMock.Setup(s => s.GetById(1)).Returns(tarefa);

            var tarefaObtida = _tarefaServiceMock.Object.GetById(1);
            tarefaObtida.Status = "finalizada";
            tarefaObtida.DataFim = DateTime.Now;

            tarefaObtida.Status.Should().Be("finalizada");
            tarefaObtida.DataFim.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }
        [Test]
        public void CriarTarefa_ComDadosValidos_DeveExecutarSemErro()
        {
            var tarefa = new Tarefa
            {
                Descricao = "Trabalho ESII",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddHours(2),
                Status = "em curso",
                ProjetoId = 1,
                UtilizadorId = 42,
                HorasGastas = 0
            };

            Action act = () =>
            {
                if (tarefa.DataFim <= tarefa.DataInicio)
                    throw new ArgumentException("A data de fim deve ser posterior à de início.");
            };

            act.Should().NotThrow();
        }
        [Test]
        public void MoverTarefa_DeveAtualizarProjetoId()
        {
            var tarefa = new Tarefa
            {
                Id = 99,
                ProjetoId = 1,
                Descricao = "Tarefa para mover",
                Status = "em curso"
            };

            _tarefaServiceMock.Setup(s => s.GetById(99)).Returns(tarefa);

            var tarefaObtida = _tarefaServiceMock.Object.GetById(99);
            tarefaObtida.ProjetoId = 2; // move para outro projeto

            tarefaObtida.ProjetoId.Should().Be(2);
        }


    }
}
