using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages.Tarefas
{
    public class TarefasEmCursoModel : PageModel
    {
        private readonly TarefaService _service;

        public List<Tarefa> Tarefas { get; set; } = new();

        [TempData]
        public string? Mensagem { get; set; }

        public TarefasEmCursoModel(TarefaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int utilizadorId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;
            Tarefas = await _service.GetTarefasEmCursoAsync(utilizadorId);
            return Page();
        }

        public async Task<IActionResult> OnPostConcluirAsync(int tarefaId)
        {
            await _service.FinalizarTarefaAsync(tarefaId);
            Mensagem = "Tarefa concluída com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoverAsync(int tarefaId)
        {
            await _service.RemoverTarefaAsync(tarefaId);
            Mensagem = "Tarefa removida com sucesso!";
            return RedirectToPage();
        }
    }
}
