using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Services
{
    public interface IProjetoService
    {
        List<ProjetoDTO> GetAll();
        ProjetoDetalhadoDTO? GetDetalhadoById(int id);
        Projeto Create(ProjetoCreateDTO dto, int userId);
        bool Update(int id, Projeto projeto);
        bool Delete(int id);
        IEnumerable<ProjetoDTO> GetByUserId(int userId);
        Projeto? GetById(int id); // ✅ Adicionado
        Projeto? GetProjetoComMembros(int projetoId);
        bool ConcluirProjeto(int id);
        decimal CalcularValorTotalProjeto(int projetoId);
        List<MembroDTO> GetMembrosDoProjeto(int projetoId);
        int ContarClientesUnicosPorUserId(int userId); // ✅ NOVO
    }
}
