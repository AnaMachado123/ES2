using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;
using front.Services;

namespace front.Pages
{
    public class CriarTarefaIndividualModel : PageModel
    {
        private readonly TarefaService _service;

        public CriarTarefaIndividualModel(TarefaService service)
        {
            _service = service;
        }

        [BindProperty]
        public Tarefa NovaTarefa { get; set; } = new();

        public List<Projeto> ProjetosDisponiveis { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ProjetosDisponiveis = await _service.GetProjetosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int utilizadorId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;
            if (utilizadorId == 0) return RedirectToPage("/Login");

            NovaTarefa.UtilizadorId = utilizadorId;
            NovaTarefa.Status = "Em curso";

            await _service.CreateTarefaAsync(NovaTarefa);
            return RedirectToPage("/Tarefas");
        }
    }
}
