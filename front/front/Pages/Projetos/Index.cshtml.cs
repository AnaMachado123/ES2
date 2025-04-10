using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class IndexModel : PageModel
    {
        public List<Projeto> Projetos { get; set; } = new();

        public void OnGet()
        {
            // Exemplo de dados estáticos
            Projetos = new List<Projeto>
            {
                new Projeto { Id = 1, Nome = "Gestão de Redes", Cliente = "Eduarda Gomes", Estado = "Em curso" },
                new Projeto { Id = 2, Nome = "Website IPVC", Cliente = "Adriana Meira", Estado = "Pendente" },
                new Projeto { Id = 3, Nome = "API Financeira", Cliente = "Diana Matos", Estado = "Concluído" }
            };
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; }
            public string? Estado { get; set; }
        }
    }
}
