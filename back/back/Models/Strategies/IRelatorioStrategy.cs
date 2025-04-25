using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public interface IRelatorioStrategy<T>
{
    T GerarRelatorio(GestaoServicosClientesContext context, int utilizadorId, int mes, int ano);
}
