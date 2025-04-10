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
            TotalProjetos = 3;
            TarefasPendentes = 4;
            TotalClientes = 2;

            Projetos = new List<Projeto>
            {
                new Projeto { Id = 1, Nome = "Gestão de Redes", Cliente = "Eduarda Gomes", Status = "Em curso" },
                new Projeto { Id = 2, Nome = "Website IPVC", Cliente = "Adriana Meira", Status = "Pendente" },
                new Projeto { Id = 3, Nome = "API Financeira", Cliente = "Diana Matos", Status = "Concluído" }
            };
        }
    }

    public class Projeto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cliente { get; set; }
        public string? Status { get; set; }
    }
}
