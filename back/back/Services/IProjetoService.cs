using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

public interface IProjetoService
{
    IEnumerable<Projeto> GetAll();
    Projeto GetById(int id);
    Projeto Create(ProjetoCreateDTO dto); // usa o DTO como no controller
    bool Update(int id, Projeto projeto);
    bool Delete(int id);
}

