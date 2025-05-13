using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Claims;
using front.Models;

namespace front.Pages
{
    public class RelatorioMensalModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty] public int Mes { get; set; }
        [BindProperty] public int Ano { get; set; }

        public List<Relatorio>? Relatorios { get; set; }

        public RelatorioMensalModel(IHttpClientFactory factory)
        {
            // "Backend" foi registado no Program.cs com a BaseAddress do API
            _httpClient = factory.CreateClient("Backend");
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            /* 1️⃣  Descobre o ID do utilizador logado a partir do token  */
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim is null)
            {
                // não devia acontecer se a página exigir login
                ModelState.AddModelError(string.Empty, "Sessão expirada — faça login novamente.");
                return Page();
            }

            int utilizadorId = int.Parse(idClaim.Value);

            /* 2️⃣  Chama o mesmo endpoint que funciona no Swagger         */
            var url = $"api/Relatorio?utilizadorId={utilizadorId}&mes={Mes}&ano={Ano}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Relatorios = await response.Content.ReadFromJsonAsync<List<Relatorio>>();
                if (Relatorios is null || Relatorios.Count == 0)
                    ModelState.AddModelError(string.Empty, "Nenhum relatório encontrado para o mês e ano seleccionados.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao consultar o relatório.");
            }

            return Page();
        }
    }
}
