using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using front.Models;


namespace front.Pages;

public class RelatorioMensalModel : PageModel
{
    private readonly HttpClient _httpClient;

    [BindProperty]
    public int Mes { get; set; }

    [BindProperty]
    public int Ano { get; set; }

    public Relatorio? Relatorio { get; set; }

    public List<Relatorio>? Relatorios { get; set; }

    public RelatorioMensalModel(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("Backend");
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        int utilizadorId = 1; // <- este é o ID hardcoded por agora

        var response = await _httpClient.GetAsync(
            $"api/relatoriostrategy/mensal/lista?utilizadorId={utilizadorId}&mes={Mes}&ano={Ano}");

        if (response.IsSuccessStatusCode)
        {
            Relatorios = await response.Content.ReadFromJsonAsync<List<Relatorio>>();
        }

        return Page();
    }
}
