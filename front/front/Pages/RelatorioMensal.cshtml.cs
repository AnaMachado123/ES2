using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Claims;
using front.Models;

namespace front.Pages;

public class RelatorioMensalModel : PageModel
{
    private readonly HttpClient _httpClient;

    [BindProperty] public int Mes { get; set; }
    [BindProperty] public int Ano { get; set; }

    public int UtilizadorId { get; set; }
    public List<RelatorioMensalDTO>? RelatoriosMensais { get; set; }

    public RelatorioMensalModel(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("Backend");
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim is null)
        {
            ModelState.AddModelError(string.Empty, "Sessão expirada — faça login novamente.");
            return Page();
        }

        UtilizadorId = int.Parse(idClaim.Value);

        var url = $"api/relatorio/mensal?utilizadorId={UtilizadorId}&mes={Mes}&ano={Ano}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            RelatoriosMensais = await response.Content.ReadFromJsonAsync<List<RelatorioMensalDTO>>();
            if (RelatoriosMensais is null || RelatoriosMensais.Count == 0)
                ModelState.AddModelError(string.Empty, "Nenhum relatório encontrado para o mês e ano seleccionados.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Erro ao consultar o relatório.");
        }

        return Page();
    }
}
