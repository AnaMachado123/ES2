using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioContextProjeto<T>
{
    private IRelatorioProjetoStrategy<T> _strategy;

    public RelatorioContextProjeto(IRelatorioProjetoStrategy<T> strategy)
    {
        _strategy = strategy;
    }

    public T Executar(GestaoServicosClientesContext context, int clienteId)
    {
        return _strategy.GerarRelatorio(context, clienteId);
    }
}
