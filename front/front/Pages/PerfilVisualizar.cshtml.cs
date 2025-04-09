using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class PerfilVisualizarModel : PageModel
    {
        public UtilizadorModel Utilizador { get; set; } = new();

        public void OnGet()
        {
            // Simulação de dados
            Utilizador = new UtilizadorModel
            {
                Nome = "Ana Machado",
                Email = "ana@email.com",
                CargaHorariaDiaria = 8
            };
        }

        public class UtilizadorModel
        {
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int CargaHorariaDiaria { get; set; }
        }
    }
}
