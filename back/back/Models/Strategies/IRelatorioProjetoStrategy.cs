using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public interface IRelatorioProjetoStrategy<T>
{
    T GerarRelatorio(GestaoServicosClientesContext context, int projetoId);
}
