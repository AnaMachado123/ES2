using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace front.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string TipoUtilizador { get; set; } = "regular";
        public int TotalProjetos { get; set; }
        public int TarefasPendentes { get; set; } = 0;
        public int TotalClientes { get; set; } = 0;
        public List<Projeto> Projetos { get; set; } = new();

        public async Task OnGetAsync()
        {
            TipoUtilizador = HttpContext.Session.GetString("Tipo") ?? "regular";

            try
            {
                var client = _httpClientFactory.CreateClient("Backend");
                var response = await client.GetAsync("api/Projeto");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var projetosApi = JsonSerializer.Deserialize<List<Projeto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (projetosApi != null)
                    {
                        Projetos = projetosApi;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar projetos: {ex.Message}");
            }

            TotalProjetos = Projetos.Count;
            TarefasPendentes = Projetos.Count(p => p.Status == "Pendente");
            TotalClientes = Projetos.Select(p => p.Cliente).Distinct().Count();
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; } = "Desconhecido";
            public string? Estado { get; set; } = "Indefinido";
            public int ClienteId { get; set; }

            // Manter compatibilidade com código antigo que usa "Status"
            public string? Status => Estado;
        }

    }
}
