using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;


namespace BackendTesteESII.Services
{
    public interface IConviteService
    {
        IEnumerable<Convite> GetAll();
        Convite GetById(int id);
        Convite Create(Convite convite);
        bool Update(int id, Convite convite);
        bool Delete(int id);
    }
}
