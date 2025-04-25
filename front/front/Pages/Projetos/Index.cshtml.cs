using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace front.Pages.Projetos
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Projeto> Projetos { get; set; } = new();

        public async Task OnGetAsync()
        {
            var exemplos = new List<Projeto>
            {
                new Projeto { Id = 1, Nome = "Gestão de Redes", Cliente = "Eduarda Gomes", Estado = "Em curso", Descricao = "Sistema de Gestão de Redes para otimizar...", DataInicio = new DateTime(2025, 1, 15), DataFim = new DateTime(2025, 3, 1), HorasTrabalho = 140, UtilizadorId = 99, ClienteId = 101 },
                new Projeto { Id = 2, Nome = "Website IPVC", Cliente = "Adriana Meira", Estado = "Pendente", Descricao = "Desenvolvimento do novo site do IPVC.", DataInicio = new DateTime(2025, 2, 5), DataFim = null, HorasTrabalho = 50, UtilizadorId = 100, ClienteId = 102 },
                new Projeto { Id = 3, Nome = "API Financeira", Cliente = "Diana Matos", Estado = "Concluído", Descricao = "Desenvolvimento de API para gestão financeira.", DataInicio = new DateTime(2024, 5, 1), DataFim = new DateTime(2024, 11, 15), HorasTrabalho = 220, UtilizadorId = 101, ClienteId = 103 }
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
                        Projetos = projetosApi.Concat(exemplos).ToList(); // ✅ API vem antes dos exemplos
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
                Console.WriteLine($"Erro ao buscar projetos da API: {ex.Message}");
                Projetos = exemplos; // fallback se falhar a API
            }
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cliente { get; set; } = "Desconhecido";
            public string Estado { get; set; } = "Indefinido";
            public string Descricao { get; set; } = "";
            public DateTime DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int HorasTrabalho { get; set; }
            public int UtilizadorId { get; set; }
            public int ClienteId { get; set; }
        }
    }
}
