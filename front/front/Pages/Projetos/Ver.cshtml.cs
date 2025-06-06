using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public ProjetoDetalhadoDTO Projeto { get; set; } = new();
        public List<MembroDTO> Membros { get; set; } = new();
        public decimal ValorTotal { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/Projeto/{id}/detalhado");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            Projeto = JsonSerializer.Deserialize<ProjetoDetalhadoDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var responseMembros = await client.GetAsync($"api/Projeto/{id}/membros");
            if (responseMembros.IsSuccessStatusCode)
            {
                var jsonMembros = await responseMembros.Content.ReadAsStringAsync();
                Membros = JsonSerializer.Deserialize<List<MembroDTO>>(jsonMembros, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }

            var responseValor = await client.GetAsync($"api/Projeto/{id}/valor");
            if (responseValor.IsSuccessStatusCode)
            {
                var jsonValor = await responseValor.Content.ReadAsStringAsync();
                var valorObj = JsonDocument.Parse(jsonValor).RootElement;

                // ✅ Usa chave correta: valorTotal (minúsculo)
                if (valorObj.TryGetProperty("valorTotal", out var valor))
                {
                    ValorTotal = valor.GetDecimal();
                }
                else
                {
                    ValorTotal = 0; // fallback seguro
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConcluirAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"api/Projeto/{id}/concluir", null);
            if (!response.IsSuccessStatusCode) return RedirectToPage("/Erro");

            return RedirectToPage(new { id });
        }

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

        public class MembroDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }
    }
}
