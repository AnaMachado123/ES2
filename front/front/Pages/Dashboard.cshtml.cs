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

            var exemplos = new List<Projeto>
            {
                new Projeto { Id = 1, Nome = "Gestão de Redes", Cliente = "Eduarda Gomes", Status = "Em curso" },
                new Projeto { Id = 2, Nome = "Website IPVC", Cliente = "Adriana Meira", Status = "Pendente" },
                new Projeto { Id = 3, Nome = "API Financeira", Cliente = "Diana Matos", Status = "Concluído" }
            };

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
                        Projetos = projetosApi.Concat(exemplos).ToList(); // 👈 API first
                    }
                    else
                    {
                        Projetos = exemplos;
                    }
                }
                else
                {
                    Projetos = exemplos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar projetos: {ex.Message}");
                Projetos = exemplos;
            }

            TotalProjetos = Projetos.Count;
            TarefasPendentes = Projetos.Count(p => p.Status == "Pendente"); // Exemplo básico
            TotalClientes = Projetos.Select(p => p.Cliente).Distinct().Count();
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; } = "Desconhecido";
            public string? Status { get; set; } = "Indefinido";
        }
    }
}
