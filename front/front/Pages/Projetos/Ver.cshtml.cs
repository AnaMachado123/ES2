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

        // ✅ Propriedade da página (sem BindProperty porque é só leitura)
        public ProjetoDetalhadoDTO Projeto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");

            var response = await client.GetAsync($"api/Projeto/{id}/detalhado");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();

            var projetoApi = JsonSerializer.Deserialize<ProjetoDetalhadoDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (projetoApi == null)
                return NotFound();

            Projeto = projetoApi;
            return Page();
        }

        // DTO usado na página
        public class ProjetoDetalhadoDTO
        {
            public string Nome { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public int HorasTrabalho { get; set; }
            public string NomeCliente { get; set; } = string.Empty;
            public string NomeCriador { get; set; } = string.Empty;
            public List<TarefaHistoricoDTO> Tarefas { get; set; } = new();
        }

        public class TarefaHistoricoDTO
        {
            public string Descricao { get; set; } = string.Empty;
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public string Estado { get; set; } = string.Empty;
            public int HorasGastas { get; set; }
        }
    }
}
