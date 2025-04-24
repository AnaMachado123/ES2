using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly GestaoServicosClientesContext _context;

        public TarefaService(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        public IEnumerable<Tarefa> GetAll() => _context.Tarefas.ToList();

        public Tarefa GetById(int id) => _context.Tarefas.Find(id);

        public Tarefa Create(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return tarefa;
        }

        public bool Update(int id, Tarefa tarefa)
        {
            var existente = _context.Tarefas.Find(id);
            if (existente == null) return false;

            existente.Descricao = tarefa.Descricao;
            existente.DataInicio = tarefa.DataInicio;
            existente.DataFim = tarefa.DataFim;
            existente.Status = tarefa.Status;
            existente.ProjetoId = tarefa.ProjetoId;
            existente.UtilizadorId = tarefa.UtilizadorId;
            existente.HorasGastas = tarefa.HorasGastas;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            _context.SaveChanges();
            return true;
        }
    }
}
