using BackendTesteESII.Data;
using BackendTesteESII.Models;
using System.Collections.Generic;
using System.Linq;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioMensalStrategy : IRelatorioStrategy<List<Relatorio>>
{
    public List<Relatorio> GerarRelatorio(GestaoServicosClientesContext context, int utilizadorId, int mes, int ano)
    {
        return context.Relatorios
            .Where(r => r.UtilizadorId == utilizadorId && r.Mes == mes && r.Ano == ano)
            .ToList();
    }
}
