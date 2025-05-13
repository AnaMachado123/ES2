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
                Console.WriteLine($"Erro ao buscar projetos da API: {ex.Message}");
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
