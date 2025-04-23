using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public interface IRelatorioStrategy
{
    string GerarRelatorio(GestaoServicosClientesContext context, int parametro);
}
