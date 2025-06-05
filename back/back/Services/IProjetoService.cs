using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;
using System.Collections.Generic;

namespace BackendTesteESII.Services
{
    public interface IProjetoService
    {
        List<ProjetoDTO> GetAll();
        //Projeto GetById(int id);
        ProjetoDetalhadoDTO? GetDetalhadoById(int id);

        Projeto Create(ProjetoCreateDTO dto, int userId); // ← passa o userId diretamente

        bool Update(int id, Projeto projeto);
        bool Delete(int id);
        IEnumerable<ProjetoDTO> GetByUserId(int userId);


    }
}
