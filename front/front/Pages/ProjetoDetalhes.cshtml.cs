using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages;

public class ProjetoDetalhesModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProjetoDetalhesModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public ProjetoDetalhadoDTO Projeto { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var token = HttpContext.Session.GetString("AuthToken");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }

        var client = _httpClientFactory.CreateClient("Backend");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"api/Projeto/{id}/detalhes");

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Erro");
        }

        var json = await response.Content.ReadAsStringAsync();
        Projeto = JsonSerializer.Deserialize<ProjetoDetalhadoDTO>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return Page();
    }

    public class ProjetoDetalhadoDTO
    {
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int HorasTrabalho { get; set; }
        public string NomeCliente { get; set; } = "";
        public string NomeCriador { get; set; } = "";
        public List<TarefaHistoricoDTO> Tarefas { get; set; } = new();
    }

    public class TarefaHistoricoDTO
    {
        public string Descricao { get; set; } = "";
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; } = "";
        public int HorasGastas { get; set; }
        public string NomeUtilizador { get; set; } = "";
    }
}
