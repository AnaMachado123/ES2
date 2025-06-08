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

        [TempData]
        public string? Mensagem { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ProjetosDisponiveis = await _service.GetProjetosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int utilizadorId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;
            if (utilizadorId == 0) return RedirectToPage("/Login");

            // ✅ Se não for selecionado nenhum projeto, forçar null
            if (NovaTarefa.ProjetoId == 0)
                NovaTarefa.ProjetoId = null;

            NovaTarefa.UtilizadorId = utilizadorId;
            NovaTarefa.Status = "Em curso";

            // ✅ Forçar datas para UTC (PostgreSQL exige isso)
            NovaTarefa.DataInicio = DateTime.SpecifyKind(NovaTarefa.DataInicio, DateTimeKind.Utc);
            if (NovaTarefa.DataFim.HasValue)
                NovaTarefa.DataFim = DateTime.SpecifyKind(NovaTarefa.DataFim.Value, DateTimeKind.Utc);

            // ✅ Verifica o estado do modelo
            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState inválido:");
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"→ {entry.Key}: {error.ErrorMessage}");
                    }
                }

                ProjetosDisponiveis = await _service.GetProjetosAsync();
                return Page();
            }

            var sucesso = await _service.CreateTarefaAsync(NovaTarefa);

            if (!sucesso)
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar tarefa.");
                ProjetosDisponiveis = await _service.GetProjetosAsync();
                return Page();
            }

            Mensagem = "Tarefa criada com sucesso!";
            return RedirectToPage("/Tarefas");
        }
    }
}
