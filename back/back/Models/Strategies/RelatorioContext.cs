using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioContext
{
    private IRelatorioStrategy _strategy;

    public RelatorioContext(IRelatorioStrategy strategy)
    {
        _strategy = strategy;
    }

    public string Executar(GestaoServicosClientesContext context, int parametro)
    {
        return _strategy.GerarRelatorio(context, parametro);
    }
}
