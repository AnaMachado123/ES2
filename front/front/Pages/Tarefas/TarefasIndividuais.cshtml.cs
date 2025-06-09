using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages.Tarefas
{
    public class TarefasIndividuaisModel : PageModel
    {
        private readonly TarefaService _tarefaService;

        public List<Tarefa> Tarefas { get; set; } = new();

        [TempData]
        public string? Mensagem { get; set; }

        public TarefasIndividuaisModel(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        public async Task OnGetAsync()
        {
            int utilizadorId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;
            Tarefas = await _tarefaService.GetTarefasIndividuaisAsync(utilizadorId);
        }

        public async Task<IActionResult> OnPostConcluirAsync(int tarefaId)
        {
            await _tarefaService.FinalizarTarefaAsync(tarefaId);
            Mensagem = "Tarefa concluída com sucesso!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoverAsync(int tarefaId)
        {
            await _tarefaService.RemoverTarefaAsync(tarefaId);
            Mensagem = "Tarefa removida com sucesso!";
            return RedirectToPage();
        }
    }
}
