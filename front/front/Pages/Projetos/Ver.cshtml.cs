using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class VerModel : PageModel
    {
        [BindProperty]
        public ProjetoInfo Projeto { get; set; }

        public IActionResult OnGet(int id)
        {
            var projetos = new List<ProjetoInfo>
            {
                new ProjetoInfo { Id = 1, Nome = "Plataforma de Reservas", Cliente = "Ana Machado", Estado = "Concluído" },
                new ProjetoInfo { Id = 2, Nome = "Sistema de Gestão Escolar", Cliente = "Carlos Silva", Estado = "Em desenvolvimento" },
                new ProjetoInfo { Id = 3, Nome = "Site para Clínica", Cliente = "Joana Reis", Estado = "A iniciar" }
            };

            Projeto = projetos.FirstOrDefault(p => p.Id == id);

            if (Projeto == null)
                return NotFound();

            return Page();
        }

        public class ProjetoInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
        }
    }
}
