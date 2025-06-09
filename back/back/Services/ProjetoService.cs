using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BackendTesteESII.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly GestaoServicosClientesContext _context;

        public ProjetoService(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        public List<ProjetoDTO> GetAll()
        {
            var projetos = _context.Projetos.ToList();
            var clientes = _context.Clientes.ToDictionary(c => c.Id, c => c.Nome);

            return projetos.Select(p => new ProjetoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Estado = p.Estado,
                Cliente = clientes.TryGetValue(p.ClienteId, out var nome) ? nome : "Desconhecido",
                DataInicio = p.DataInicio,
                DataFim = p.DataFim,
                HorasTrabalho = p.HorasTrabalho
            }).ToList();
        }

        public IEnumerable<ProjetoDTO> GetByUserId(int userId)
        {
            var clientes = _context.Clientes.ToDictionary(c => c.Id, c => c.Nome);

            var projetosCriados = _context.Projetos.Where(p => p.UtilizadorId == userId);

            var projetosAssociados = _context.UtilizadorProjetos
                .Include(up => up.Projeto)
                .Where(up => up.UtilizadorId == userId)
                .Select(up => up.Projeto);

            var todosProjetos = projetosCriados
                .Union(projetosAssociados)
                .Distinct()
                .ToList();

            return todosProjetos.Select(p => new ProjetoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Estado = p.Estado,
                Cliente = clientes.TryGetValue(p.ClienteId, out var nome) ? nome : "Desconhecido",
                DataInicio = p.DataInicio,
                DataFim = p.DataFim,
                HorasTrabalho = p.HorasTrabalho
            });
        }

        public Projeto? GetById(int id)
        {
            return _context.Projetos.FirstOrDefault(p => p.Id == id);
        }

        public ProjetoDetalhadoDTO? GetDetalhadoById(int id)
        {
            var projeto = _context.Projetos.FirstOrDefault(p => p.Id == id);
            if (projeto == null) return null;

            var clienteNome = _context.Clientes
                .Where(c => c.Id == projeto.ClienteId)
                .Select(c => c.Nome)
                .FirstOrDefault() ?? "Desconhecido";

            var utilizadorNome = _context.Utilizadores
                .Where(u => u.Id == projeto.UtilizadorId)
                .Select(u => u.Nome)
                .FirstOrDefault() ?? "Desconhecido";

            var tarefas = _context.Tarefas
                .Where(t => t.ProjetoId == projeto.Id)
                .Select(t => new TarefaHistoricoDTO
                {
                    Descricao = t.Descricao,
                    DataInicio = t.DataInicio,
                    DataFim = t.DataFim,
                    HorasGastas = t.HorasGastas,
                    Estado = t.Status
                }).ToList();

            var membros = _context.UtilizadorProjetos
                .Include(up => up.Utilizador)
                .Where(up => up.ProjetoId == projeto.Id)
                .Select(up => new MembroDTO
                {
                    Id = up.Utilizador.Id,
                    Nome = up.Utilizador.Nome,
                    Email = up.Utilizador.Email
                }).ToList();

            return new ProjetoDetalhadoDTO
            {
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                Estado = projeto.Estado,
                DataInicio = projeto.DataInicio,
                DataFim = projeto.DataFim,
                HorasTrabalho = projeto.HorasTrabalho,
                TotalHoras = tarefas.Sum(t => t.HorasGastas),
                NomeCliente = clienteNome,
                NomeCriador = utilizadorNome,
                Tarefas = tarefas,
                ClienteId = projeto.ClienteId,
                UtilizadorId = projeto.UtilizadorId,
                Membros = membros
            };
        }

        public Projeto Create(ProjetoCreateDTO dto, int userId)
        {
            var novoProjeto = new Projeto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DataInicio = dto.DataInicio.ToUniversalTime(),
                DataFim = dto.DataFim.ToUniversalTime(),
                ClienteId = dto.ClienteId,
                UtilizadorId = userId,
                Estado = dto.Estado,
                HorasTrabalho = dto.HorasTrabalho
            };

            _context.Projetos.Add(novoProjeto);
            _context.SaveChanges();

            foreach (var tarefa in dto.Tarefas)
            {
                var novaTarefa = new Tarefa
                {
                    Descricao = tarefa.Descricao,
                    DataInicio = tarefa.DataInicio.ToUniversalTime(),
                    DataFim = tarefa.DataFim.ToUniversalTime(),
                    Status = tarefa.Status,
                    HorasGastas = tarefa.HorasGastas,
                    UtilizadorId = userId,
                    ProjetoId = novoProjeto.Id
                };

                _context.Tarefas.Add(novaTarefa);
            }

            _context.SaveChanges();
            return novoProjeto;
        }

        public decimal CalcularValorTotalProjeto(int projetoId)
        {
            var projeto = _context.Projetos
                .Include(p => p.Tarefas)
                .FirstOrDefault(p => p.Id == projetoId);

            if (projeto == null) return 0;

            decimal precoHora = projeto.HorasTrabalho > 0 ? projeto.HorasTrabalho : 1;
            var totalHoras = projeto.Tarefas.Sum(t => t.HorasGastas);

            return precoHora * totalHoras;
        }

        public bool Update(int id, Projeto projeto)
        {
            var existente = _context.Projetos.Find(id);
            if (existente == null) return false;

            existente.Nome = projeto.Nome;
            existente.Descricao = projeto.Descricao;
            existente.DataInicio = projeto.DataInicio.ToUniversalTime();
            existente.DataFim = projeto.DataFim.ToUniversalTime();
            existente.ClienteId = projeto.ClienteId;
            existente.UtilizadorId = projeto.UtilizadorId;
            existente.Estado = projeto.Estado;
            existente.HorasTrabalho = projeto.HorasTrabalho;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var projeto = _context.Projetos
                .Include(p => p.Tarefas)
                .FirstOrDefault(p => p.Id == id);

            if (projeto == null) return false;

            _context.Tarefas.RemoveRange(projeto.Tarefas);
            _context.Projetos.Remove(projeto);

            _context.SaveChanges();
            return true;
        }

        public Projeto? GetProjetoComMembros(int projetoId)
        {
            return _context.Projetos
                .Include(p => p.UtilizadorProjetos)
                .ThenInclude(up => up.Utilizador)
                .FirstOrDefault(p => p.Id == projetoId);
        }

        public bool ConcluirProjeto(int id)
        {
            var projeto = _context.Projetos.FirstOrDefault(p => p.Id == id);
            if (projeto == null) return false;

            projeto.Estado = "Concluído";
            _context.SaveChanges();
            return true;
        }

        public List<MembroDTO> GetMembrosDoProjeto(int projetoId)
        {
            return _context.UtilizadorProjetos
                .Include(up => up.Utilizador)
                .Where(up => up.ProjetoId == projetoId)
                .Select(up => new MembroDTO
                {
                    Id = up.Utilizador.Id,
                    Nome = up.Utilizador.Nome,
                    Email = up.Utilizador.Email
                }).ToList();
        }

        public int ContarClientesUnicosPorUserId(int userId)
        {
            var projetosCriados = _context.Projetos
                .Where(p => p.UtilizadorId == userId);

            var projetosAssociados = _context.UtilizadorProjetos
                .Include(up => up.Projeto)
                .Where(up => up.UtilizadorId == userId)
                .Select(up => up.Projeto);

            var todosProjetos = projetosCriados
                .Union(projetosAssociados)
                .Where(p => p.ClienteId != null)
                .Distinct()
                .ToList();

            return todosProjetos
                .Select(p => p.ClienteId)
                .Distinct()
                .Count();
        }

        public int ContarTodosClientesUnicos()
        {
            return _context.Projetos
              .Where(p => p.ClienteId != null)
              .Select(p => p.ClienteId)
              .Distinct()
              .Count();
        }
    }
}
