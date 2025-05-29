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

        public Convite CreateComValidacao(int userIdLogado, ConviteCreateDTO dto)
        {
            var projeto = _context.Projetos.FirstOrDefault(p => p.Id == dto.ProjetoId);
            if (projeto == null) throw new Exception("Projeto não encontrado.");

            if (projeto.UtilizadorId != userIdLogado)
                throw new Exception("Apenas o criador do projeto pode convidar.");

            var jaExiste = _context.UtilizadorProjetos.Any(up =>
                up.ProjetoId == dto.ProjetoId && up.UtilizadorId == dto.UtilizadorId);
            if (jaExiste)
                throw new Exception("Utilizador já está no projeto.");

            var convite = new Convite
            {
                UtilizadorId = dto.UtilizadorId,
                ProjetoId = dto.ProjetoId,
                Estado = "Pendente"
            };

            _context.Convites.Add(convite);
            _context.SaveChanges();

            return convite;
        }

        public bool AceitarConvite(int conviteId)
        {
            var convite = _context.Convites.FirstOrDefault(c => c.Id == conviteId);
            if (convite == null || convite.Estado != "Pendente") return false;

            // Verifica se já está associado
            var jaExiste = _context.UtilizadorProjetos.Any(up =>
                up.UtilizadorId == convite.UtilizadorId && up.ProjetoId == convite.ProjetoId);
            if (jaExiste) return false;

            // Muda estado e associa
            convite.Estado = "Aceite";
            _context.UtilizadorProjetos.Add(new UtilizadorProjeto
            {
                UtilizadorId = convite.UtilizadorId,
                ProjetoId = convite.ProjetoId
            });

            _context.SaveChanges();
            return true;
        }
        
        public bool RecusarConvite(int conviteId)
        {
            var convite = _context.Convites.FirstOrDefault(c => c.Id == conviteId);
            if (convite == null || convite.Estado != "Pendente") return false;

            convite.Estado = "Recusado";
            _context.SaveChanges();
            return true;
        }



    }
}
