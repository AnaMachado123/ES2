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
        Cliente = clientes.TryGetValue(p.ClienteId, out var nome) ? nome : "Desconhecido"
    }).ToList();

    return lista;
}


        public Projeto GetById(int id) => _context.Projetos.Find(id);

        public Projeto Create(ProjetoCreateDTO dto)
        {
            var novoProjeto = new Projeto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DataInicio = dto.DataInicio.ToUniversalTime(),
                DataFim = dto.DataFim.ToUniversalTime(),
                ClienteId = dto.ClienteId,
                HorasTrabalho = dto.HorasTrabalho,
                UtilizadorId = dto.UtilizadorId,
                Estado = dto.Estado,
                Tarefas = dto.Tarefas.Select(t => new Tarefa
                {
                    Descricao = t.Descricao,
                    DataInicio = t.DataInicio.ToUniversalTime(),
                    DataFim = t.DataFim.ToUniversalTime(),
                    Status = t.Status,
                    HorasGastas = t.HorasGastas,
                    UtilizadorId = t.UtilizadorId
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
