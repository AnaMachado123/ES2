using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class ProjetosModel : PageModel
    {
        public List<Projeto> Projetos { get; set; } = new();

        public void OnGet()
        {
            Projetos = new List<Projeto>
            {
                new Projeto { Id = 1, Nome = "Gestão de Redes", Cliente = "Eduarda Gomes", Estado = "Em Curso" },
                new Projeto { Id = 2, Nome = "Website IPVC", Cliente = "Adriana Meira", Estado = "Pendente" },
                new Projeto { Id = 3, Nome = "API Financeira", Cliente = "Diana Matos", Estado = "Concluido" }
            };
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Cliente { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }
    }
}
