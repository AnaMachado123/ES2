using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;



namespace BackendTesteESII.Services
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetById(int id);
        Cliente Create(Cliente cliente);
        bool Update(int id, Cliente cliente);
        bool Delete(int id);
    }
}
