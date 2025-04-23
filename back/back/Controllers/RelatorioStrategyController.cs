using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
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

    [HttpGet("mensal")]
    public IActionResult GerarRelatorioMensal([FromQuery] int mes)
    {
        var contexto = new RelatorioContext(new RelatorioMensalStrategy());
        var resultado = contexto.Executar(_context, mes);
        return Ok(resultado);
    }

    [HttpGet("projeto")]
    public IActionResult GerarRelatorioProjeto([FromQuery] int clienteId)
    {
        var contexto = new RelatorioContext(new RelatorioProjetoStrategy());
        var resultado = contexto.Executar(_context, clienteId);
        return Ok(resultado);
    }

}
