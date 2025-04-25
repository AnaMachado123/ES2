using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioContext<T>
{
    private IRelatorioStrategy<T> _strategy;

    public RelatorioContext(IRelatorioStrategy<T> strategy)
    {
        _strategy = strategy;
    }

    public T Executar(GestaoServicosClientesContext context, int utilizadorId, int mes, int ano)
    {
        return _strategy.GerarRelatorio(context, utilizadorId, mes, ano);
    }
}
