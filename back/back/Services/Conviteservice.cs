using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Services
{
    public class ConviteService : IConviteService
    {
        private readonly GestaoServicosClientesContext _context;

        public ConviteService(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        public IEnumerable<Convite> GetAll() => _context.Convites.ToList();

        public Convite GetById(int id) => _context.Convites.Find(id);

        public Convite Create(Convite convite)
        {
            _context.Convites.Add(convite);
            _context.SaveChanges();
            return convite;
        }

        public bool Update(int id, Convite convite)
        {
            var existente = _context.Convites.Find(id);
            if (existente == null) return false;

            existente.UtilizadorId = convite.UtilizadorId;
            existente.ProjetoId = convite.ProjetoId;
            existente.Estado = convite.Estado;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var convite = _context.Convites.Find(id);
            if (convite == null) return false;

            _context.Convites.Remove(convite);
            _context.SaveChanges();
            return true;
        }
    }
}
