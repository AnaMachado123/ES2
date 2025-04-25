using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.Strategies;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioStrategyController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public RelatorioStrategyController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    [HttpGet("mensal/lista")]
    public ActionResult<List<Relatorio>> GetRelatoriosMensais(
        [FromQuery] int utilizadorId,
        [FromQuery] int mes,
        [FromQuery] int ano)
    {
        var contexto = new RelatorioContext<List<Relatorio>>(new RelatorioMensalStrategy());
        var resultado = contexto.Executar(_context, utilizadorId, mes, ano);
        return Ok(resultado);
    }

    [HttpGet("projeto/lista")]
    public ActionResult<List<RelatorioProjeto>> GetRelatoriosProjeto(
        [FromQuery] int ProjetoId)
    {
        var contexto = new RelatorioContextProjeto<List<RelatorioProjeto>>(new RelatorioProjetoStrategy());
        var resultado = contexto.Executar(_context, ProjetoId);
        return Ok(resultado);
    }
    
}
