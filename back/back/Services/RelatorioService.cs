using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly GestaoServicosClientesContext _context;

        public RelatorioService(GestaoServicosClientesContext context)
            => _context = context;

        public IEnumerable<Relatorio> GetAll()
            => _context.Relatorios.ToList();

        public Relatorio GetById(int id)
            => _context.Relatorios.Find(id);

        public Relatorio Create(Relatorio relatorio)
        {
            _context.Relatorios.Add(relatorio);
            _context.SaveChanges();
            return relatorio;
        }

        public bool Update(int id, Relatorio relatorio)
        {
            var existente = _context.Relatorios.Find(id);
            if (existente == null) return false;

            existente.UtilizadorId = relatorio.UtilizadorId;
            existente.Mes          = relatorio.Mes;
            existente.Ano          = relatorio.Ano;
            existente.TotalHoras   = relatorio.TotalHoras;
            existente.TotalPreco   = relatorio.TotalPreco;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var relatorio = _context.Relatorios.Find(id);
            if (relatorio == null) return false;

            _context.Relatorios.Remove(relatorio);
            _context.SaveChanges();
            return true;
        }
    }
}
