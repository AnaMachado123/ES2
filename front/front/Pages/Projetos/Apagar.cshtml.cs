using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class ApagarModel : PageModel
    {
        [BindProperty]
        public ProjetoInfo Projeto { get; set; } = new();

        public void OnGet(int id)
        {
            // Simula busca do projeto pelo ID
            Projeto = new ProjetoInfo
            {
                Id = id,
                Nome = "Projeto de Exemplo",
                Cliente = "Ana Machado",
                Estado = "Concluído"
            };
        }

        public IActionResult OnPost()
        {
            TempData["MensagemSucesso"] = $"Projeto \"{Projeto.Nome}\" foi apagado com sucesso.";
            return RedirectToPage("/Projetos/Index");
        }

        public class ProjetoInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Cliente { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }
    }
}
