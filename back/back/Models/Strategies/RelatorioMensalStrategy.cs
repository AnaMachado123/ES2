using BackendTesteESII.Data;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioMensalStrategy : IRelatorioStrategy
{
    public string GerarRelatorio(GestaoServicosClientesContext context, int mes)
    {
        var total = context.Relatorios
            .Where(r => r.Mes == mes)
            .Count();

        return $"Relatório Mensal: foram encontrados {total} registos para o mês {mes}.";
    }
}
