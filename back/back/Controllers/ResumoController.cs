using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models.DTOs;
using System.Security.Claims;

namespace BackendTesteESII.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResumoController : ControllerBase
    {
        private readonly GestaoServicosClientesContext _context;

        public ResumoController(GestaoServicosClientesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetResumo()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

            // Projetos ativos (não concluídos)
            var projetosAtivos = _context.Projetos.Count(p => p.UtilizadorId == userId && !p.Concluido);

            // Tarefas em curso
            var tarefasEmCurso = _context.Tarefas.Count(t =>
                t.UtilizadorId == userId &&
                t.Status == "em curso");

            // Horas gastas este mês
            var inicioDoMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var horasEsteMes = _context.Tarefas
                .Where(t => t.UtilizadorId == userId && t.DataInicio >= inicioDoMes)
                .Sum(t => t.HorasGastas);

            var resumo = new ResumoDTO
            {
                ProjetosAtivos = projetosAtivos,
                TarefasEmCurso = tarefasEmCurso,
                HorasGastasEsteMes = horasEsteMes
            };

            return Ok(resumo);
        }
    }
}
