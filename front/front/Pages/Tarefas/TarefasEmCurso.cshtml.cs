using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages.Tarefas
{
    public class TarefasEmCursoModel : PageModel
    {
        private readonly TarefaService _service;

        public List<Tarefa> Tarefas { get; set; } = new();

        public TarefasEmCursoModel(TarefaService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            int utilizadorId = 1; // Substituir por ID dinâmico mais tarde
            Tarefas = await _service.GetTarefasEmCursoAsync(utilizadorId);
        }
    }
}
