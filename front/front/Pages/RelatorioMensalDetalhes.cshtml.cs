using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;

namespace front.Pages;

public class RelatorioMensalDetalhesModel : PageModel
{
    private readonly HttpClient _httpClient;

    public Relatorio Relatorio { get; set; }

    public RelatorioMensalDetalhesModel(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("Backend");
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/relatorio/{id}");

        if (!response.IsSuccessStatusCode)
            return RedirectToPage("/RelatorioMensal"); // fallback

        Relatorio = await response.Content.ReadFromJsonAsync<Relatorio>();
        return Page();
    }
}
