using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Services
{
    public class ClienteService : IClienteService
    {
        private readonly GestaoServicosClientesContext _context;

        public ClienteService(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente> GetAll() => _context.Clientes.ToList();

        public Cliente GetById(int id) => _context.Clientes.Find(id);

        public Cliente Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }

        public bool Update(int id, Cliente cliente)
        {
            var existente = _context.Clientes.Find(id);
            if (existente == null) return false;

            existente.Nome = cliente.Nome;
            existente.Email = cliente.Email;
            existente.Telefone = cliente.Telefone;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            return true;
        }
    }
}
