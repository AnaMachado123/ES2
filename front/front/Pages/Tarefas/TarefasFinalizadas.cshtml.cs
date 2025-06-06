using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages.Tarefas
{
    public class TarefasFinalizadasModel : PageModel
    {
        private readonly TarefaService _service;

        public List<Tarefa> Tarefas { get; set; } = new();

        public TarefasFinalizadasModel(TarefaService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            int utilizadorId = 1; // ⚠️ substituir por ID dinâmico quando tiveres sessão/login
            Tarefas = await _service.GetTarefasFinalizadasAsync(utilizadorId);
        }
    }
}
