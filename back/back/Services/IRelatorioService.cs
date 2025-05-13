using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;



namespace BackendTesteESII.Services
{
    // Define um contrato único: só lida com operações de Relatório
    public interface IRelatorioService
    {
        IEnumerable<Relatorio> GetAll();
        Relatorio GetById(int id);
        Relatorio Create(Relatorio relatorio);
        bool Update(int id, Relatorio relatorio);
        bool Delete(int id);

        GestaoServicosClientesContext GetContext();

    }
}
