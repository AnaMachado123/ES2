using BackendTesteESII.Data;
using BackendTesteESII.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioMensalStrategy : IRelatorioStrategy<List<RelatorioMensalDTO>>
{
    public List<RelatorioMensalDTO> GerarRelatorio(GestaoServicosClientesContext context, int utilizadorId, int mes, int ano)
    {
        var tarefas = context.Tarefas
            .Include(t => t.Projeto)
            .Where(t => t.UtilizadorId == utilizadorId
                && t.DataFim.Month == mes
                && t.DataFim.Year == ano
                && t.Status.ToLower() == "finalizada")
            .ToList();

        var horasLimiteDiaria = context.Utilizadores
            .Where(u => u.Id == utilizadorId)
            .Select(u => u.HorasDia)
            .FirstOrDefault();

        var resultado = tarefas
            .GroupBy(t => t.DataFim.Date)
            .Select(g => new RelatorioMensalDTO
            {
                Dia = g.Key.Day,
                TotalHoras = g.Sum(t => t.HorasGastas),
                TotalPreco = g.Sum(t => t.HorasGastas * (t.Projeto?.HorasTrabalho ?? 0)),
                ExcedeuLimite = g.Sum(t => t.HorasGastas) > horasLimiteDiaria,
                NomeProjeto = g.Select(t => t.Projeto.Nome).FirstOrDefault()
            })
            .OrderBy(r => r.Dia)
            .ToList();

        return resultado;
    }
}
