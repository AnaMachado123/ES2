using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Services
{
    public class RelatorioProjetoService : IRelatorioProjetoService
    {
        private readonly GestaoServicosClientesContext _context;

        public RelatorioProjetoService(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        public IEnumerable<RelatorioProjeto> GetAll() => _context.RelatorioProjetos.ToList();

        public RelatorioProjeto GetById(int id) => _context.RelatorioProjetos.Find(id);

        public RelatorioProjeto Create(RelatorioProjeto relatorio)
        {
            _context.RelatorioProjetos.Add(relatorio);
            _context.SaveChanges();
            return relatorio;
        }

        public bool Update(int id, RelatorioProjeto relatorio)
        {
            var existente = _context.RelatorioProjetos.Find(id);
            if (existente == null) return false;

            existente.ProjetoId = relatorio.ProjetoId;
            existente.ClienteId = relatorio.ClienteId;
            existente.TotalHoras = relatorio.TotalHoras;
            existente.TotalPreco = relatorio.TotalPreco;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var relatorio = _context.RelatorioProjetos.Find(id);
            if (relatorio == null) return false;

            _context.RelatorioProjetos.Remove(relatorio);
            _context.SaveChanges();
            return true;
        }
    }
}
