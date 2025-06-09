using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models.DTOs;
using System.Security.Claims;

namespace BackendTesteESII.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            // ✅ Corrigido: conta projetos diretamente da tabela projeto
            var projetosAtivos = _context.Projetos
                .Count(p => p.UtilizadorId == userId);

            var tarefasEmCurso = _context.Tarefas
                .Count(t => t.UtilizadorId == userId && t.Status.ToLower() == "em curso");

            var inicioDoMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var horasEsteMes = _context.Tarefas
                .Where(t => t.UtilizadorId == userId && t.DataInicio >= inicioDoMes)
                .Sum(t => (int?)t.HorasGastas) ?? 0;

            var dto = new ResumoDTO
            {
                ProjetosAtivos = projetosAtivos,
                TarefasEmCurso = tarefasEmCurso,
                HorasGastasEsteMes = horasEsteMes
            };

            return Ok(dto);
        }
    }
}
