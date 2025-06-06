using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages
{
    public class TarefasModel : PageModel
    {
        private readonly TarefaService _service;

        public List<Tarefa> TarefasEmCurso { get; set; } = new();
        public List<Tarefa> TarefasFinalizadas { get; set; } = new();

        public TarefasModel(TarefaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int utilizadorId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;

            Console.WriteLine("Entrou em Tarefas - ID recuperado da sessão: " + utilizadorId); // 🟢 DEBUG

            if (utilizadorId == 0)
            {
                Console.WriteLine("ID inválido. Sessão não encontrada ou expirada.");
                // Podes redirecionar ou apenas carregar vazio
            }

            TarefasEmCurso = await _service.GetTarefasEmCursoAsync(utilizadorId);
            TarefasFinalizadas = await _service.GetTarefasFinalizadasAsync(utilizadorId);

            return Page();
        }
    }
}
