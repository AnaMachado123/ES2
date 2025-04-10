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
                new Projeto { Id = 1, Nome = "Plataforma de Reservas", Cliente = "Ana Machado", Estado = "Concluído" },
                new Projeto { Id = 2, Nome = "Sistema de Gestão Escolar", Cliente = "Carlos Silva", Estado = "Em desenvolvimento" },
                new Projeto { Id = 3, Nome = "Site para Clínica", Cliente = "Joana Reis", Estado = "A iniciar" }
            };
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
        }
    }
}
