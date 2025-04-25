using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;


namespace BackendTesteESII.Services
{
    public interface IUtilizadorService
    {
        IEnumerable<Utilizador> GetAll();
        bool Update(int id, Utilizador utilizador);
        bool Delete(int id);
        string? VerificarPermissao(int id);
        Utilizador Create(UtilizadorCreateDTO dto);
        Utilizador? GetById(int id);
        bool RecuperarPassword(string email);


    }
}
