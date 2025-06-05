using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

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

            var lista = projetos.Select(p => new ProjetoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Estado = p.Estado,
                //Cliente = clientes.TryGetValue(p.ClienteId, out var nome) ? nome : "Desconhecido"
                Cliente = p.ClienteId.ToString()

            }).ToList();

            return lista;
        }

        public IEnumerable<ProjetoDTO> GetByUserId(int userId)
        {
            var clientes = _context.Clientes.ToDictionary(c => c.Id, c => c.Nome);

            var projetos = _context.Projetos
                .Where(p => p.UtilizadorId == userId)
                .ToList();

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
            });
        }




        //public Projeto GetById(int id) => _context.Projetos.Find(id);
        public ProjetoDetalhadoDTO? GetDetalhadoById(int id)
        {
            var projeto = _context.Projetos
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (projeto == null)
                return null;

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

            return new ProjetoDetalhadoDTO
            {
                Nome = projeto.Nome,
                Descricao = projeto.Descricao,
                Estado = projeto.Estado,
                DataInicio = projeto.DataInicio,
                DataFim = projeto.DataFim,
                HorasTrabalho = projeto.HorasTrabalho,
                NomeCliente = clienteNome,
                NomeCriador = utilizadorNome,
                Tarefas = tarefas
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
                HorasTrabalho = dto.HorasTrabalho,
                //UtilizadorId = dto.UtilizadorId,
                UtilizadorId = userId,
                Estado = dto.Estado,
                Tarefas = dto.Tarefas.Select(t => new Tarefa
                {
                    Descricao = t.Descricao,
                    DataInicio = t.DataInicio.ToUniversalTime(),
                    DataFim = t.DataFim.ToUniversalTime(),
                    Status = t.Status,
                    HorasGastas = t.HorasGastas,
                    UtilizadorId = userId
                }).ToList()
            };

            _context.Projetos.Add(novoProjeto);
            _context.SaveChanges();

            return novoProjeto;
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
            existente.HorasTrabalho = projeto.HorasTrabalho;
            existente.UtilizadorId = projeto.UtilizadorId;
            existente.Estado = projeto.Estado;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var projeto = _context.Projetos.Find(id);
            if (projeto == null) return false;

            _context.Projetos.Remove(projeto);
            _context.SaveChanges();
            return true;
        }
    }
}
