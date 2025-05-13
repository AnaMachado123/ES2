
using BackendTesteESII.Models;
using System.Collections.Generic;
using System.Linq;

using BackendTesteESII.Data;
using BackendTesteESII.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackendTesteESII.Models.Strategies;

public class RelatorioProjetoStrategy : IRelatorioProjetoStrategy<List<RelatorioProjetoDTO>>
{
    public List<RelatorioProjetoDTO> GerarRelatorio(GestaoServicosClientesContext context, int projetoId)
    {
        var tarefas = context.Tarefas
            .Include(t => t.Projeto)
            .Include(t => t.Utilizador)
            .Where(t => t.ProjetoId == projetoId && t.Status.ToLower() == "finalizada")
            .ToList();

        var resultado = tarefas
            .GroupBy(t => new { t.Utilizador.Nome, Data = t.DataFim.Date })
            .Select(g => new RelatorioProjetoDTO
            {
                UtilizadorNome = g.Key.Nome,
                Data = g.Key.Data,
                Horas = g.Sum(t => t.HorasGastas),
                Preco = g.Sum(t => t.HorasGastas * (t.Projeto.HorasTrabalho))
            })
            .OrderBy(r => r.Data)
            .ToList();

        return resultado;
    }
}

