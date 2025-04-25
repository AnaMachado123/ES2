using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;

namespace front.Pages.Tarefas
{
    public class TarefasFinalizadasModel : PageModel
    {
        public List<Tarefa> Tarefas { get; set; } = new();

        public void OnGet()
        {
            Tarefas = Tarefa.Exemplo().Where(t => t.Concluida).ToList();
        }
    }
}
