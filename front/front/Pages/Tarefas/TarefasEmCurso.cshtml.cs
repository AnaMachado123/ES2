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
    }
}
