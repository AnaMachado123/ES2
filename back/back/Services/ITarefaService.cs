using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;



namespace BackendTesteESII.Services
{
    public interface ITarefaService
    {
        IEnumerable<Tarefa> GetAll();
        Tarefa GetById(int id);
        Tarefa Create(Tarefa tarefa);
        bool Update(int id, Tarefa tarefa);
        bool Delete(int id);
    }
}
