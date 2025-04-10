using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class EditarModel : PageModel
    {
        [BindProperty]
        public ProjetoModel Projeto { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            // Simulação de busca do projeto (poderia vir da base de dados)
            Projeto = new ProjetoModel
            {
                Id = id,
                Nome = "Gestão de Redes",
                Descricao = "Sistema de Gestão de Redes desenvolvido...",
                ClienteId = 101,
                UtilizadorId = 99,
                Estado = "Em Curso",
                DataInicio = new DateTime(2025, 1, 15),
                // DataFim só virá se for concluído
                HorasTrabalho = 140
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Projeto.Estado == "Concluído" && Projeto.DataFim == null)
            {
                ModelState.AddModelError("Projeto.DataFim", "A data de fim é obrigatória se o estado for 'Concluído'.");
            }

            if (!ModelState.IsValid)
                return Page();

            // Aqui salvavas na base de dados ou enviavas para o backend

            return RedirectToPage("/Projetos/Index");
        }

        public class ProjetoModel
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = string.Empty;
            public DateTime DataInicio { get; set; }
            public DateTime? DataFim { get; set; }  // Opcional
            public int HorasTrabalho { get; set; }
        }
    }
}
