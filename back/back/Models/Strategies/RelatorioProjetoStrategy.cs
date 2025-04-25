using BackendTesteESII.Data;
using BackendTesteESII.Models;
using System.Collections.Generic;
using System.Linq;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioProjetoStrategy : IRelatorioProjetoStrategy<List<RelatorioProjeto>>
{
    public List<RelatorioProjeto> GerarRelatorio(GestaoServicosClientesContext context, int clienteId)
    {
        return context.RelatoriosProjeto
            .Where(p => p.ClienteId == clienteId)
            .ToList();
    }
}
