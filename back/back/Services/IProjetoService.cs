using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using System.Collections.Generic;

namespace BackendTesteESII.Services
{
    public interface IProjetoService
    {
        List<ProjetoDTO> GetAll();
        Projeto GetById(int id);
        Projeto Create(ProjetoCreateDTO dto);
        bool Update(int id, Projeto projeto);
        bool Delete(int id);
    }
}
