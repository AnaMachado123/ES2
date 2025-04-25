using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;


namespace BackendTesteESII.Services
{
    public class UtilizadorService : IUtilizadorService
    {
        private readonly GestaoServicosClientesContext _context;
        private readonly IEmailService _emailService;

        public UtilizadorService(GestaoServicosClientesContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IEnumerable<Utilizador> GetAll() => _context.Utilizadores.ToList();

        public Utilizador? GetById(int id) => _context.Utilizadores.Find(id);

        public Utilizador Create(UtilizadorCreateDTO dto)
        {
            var novo = new Utilizador
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Password = dto.Password,
                HorasDia = 8,
                IsAdmin = dto.Tipo.Equals("admin", StringComparison.OrdinalIgnoreCase)
            };

            _context.Utilizadores.Add(novo);
            _context.SaveChanges();
            return novo;
        }

        public bool Update(int id, Utilizador utilizador)
        {
            var existente = _context.Utilizadores.Find(id);
            if (existente == null) return false;

            existente.Nome = utilizador.Nome;
            existente.Email = utilizador.Email;
            existente.Password = utilizador.Password;
            existente.HorasDia = utilizador.HorasDia;
            existente.IsAdmin = utilizador.IsAdmin;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var utilizador = _context.Utilizadores.Find(id);
            if (utilizador == null) return false;

            _context.Utilizadores.Remove(utilizador);
            _context.SaveChanges();
            return true;
        }

        public string? VerificarPermissao(int id)
        {
            var userFromDb = _context.Utilizadores.FirstOrDefault(u => u.Id == id);
            if (userFromDb == null) return null;

            var utilizador = UtilizadorFactory.Criar(userFromDb);

            return utilizador.PodeGerirUtilizadores()
                ? "Este utilizador tem permissão para realizar a gestão de outros utilizadores."
                : "Este utilizador NÃO tem permissão para realizar a gestão de outros utilizadores.";
        }


        public bool RecuperarPassword(string email)
        {
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.Email == email);
            if (utilizador == null) return false;

            var novaPassword = Guid.NewGuid().ToString().Substring(0, 8); // exemplo
            utilizador.Password = novaPassword;

            _context.SaveChanges();

            _emailService.EnviarEmail(email, "Recuperação de Password", $"A sua nova password é: {novaPassword}");
            return true;
        }



    }
}
