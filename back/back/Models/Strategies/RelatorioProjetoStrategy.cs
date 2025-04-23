using BackendTesteESII.Data;
using System.Linq;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioProjetoStrategy : IRelatorioStrategy
{
    public string GerarRelatorio(GestaoServicosClientesContext context, int clienteId)
    {
        var total = context.RelatoriosProjeto
            .Where(p => p.ClienteId == clienteId)
            .Count();

        return $"Relatório de Projeto: foram encontrados {total} projetos para o cliente com ID {clienteId}.";
    }
}
