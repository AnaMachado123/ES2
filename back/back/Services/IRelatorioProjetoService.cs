using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;


namespace BackendTesteESII.Services
{
    public interface IRelatorioProjetoService
    {
        IEnumerable<RelatorioProjeto> GetAll();
        RelatorioProjeto GetById(int id);
        RelatorioProjeto Create(RelatorioProjeto relatorio);
        bool Update(int id, RelatorioProjeto relatorio);
        bool Delete(int id);
    }
}
