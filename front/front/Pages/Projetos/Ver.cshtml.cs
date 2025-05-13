using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace front.Pages.Projetos
{
    public class VerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ProjetoInfo Projeto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");

            var response = await client.GetAsync($"api/Projeto/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var projetoApi = JsonSerializer.Deserialize<ProjetoInfo>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (projetoApi == null)
            {
                return NotFound();
            }

            Projeto = projetoApi;
            return Page();
        }

        public class ProjetoInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Cliente { get; set; } = string.Empty;
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public DateTime DataInicio { get; set; }
            public DateTime? DataFim { get; set; } // ✅ agora nullable
            public int HorasTrabalho { get; set; }
        }
    }
}
