using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class DashboardModel : PageModel
    {
        public int TotalProjetos { get; set; }
        public int TarefasPendentes { get; set; }
        public int TotalClientes { get; set; }

        public required List<Projeto> Projetos { get; set; }

        public void OnGet()
        {
            // Dados ainda não estão a ser buscados do backend
            TotalProjetos = 0;
            TarefasPendentes = 0;
            TotalClientes = 0;
            Projetos = new List<Projeto>();
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; }
            public string? Status { get; set; }
        }
    }
}
