using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace front.Pages
{
    public class MeusConvitesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MeusConvitesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ConviteDTO> Convites { get; set; } = new();

        public class ConviteDTO
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = string.Empty;
            public string ProjetoNome { get; set; } = "";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UtilizadorId");
            if (userId == null) return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");

            var response = await client.GetAsync($"api/Convite/utilizador/{userId.Value}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var convites = JsonSerializer.Deserialize<List<ConviteDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Convites = convites?.Where(c => c.Estado == "Pendente").ToList() ?? new();
            }

            return Page();
        }

        /*public async Task<IActionResult> OnPostAceitarAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var content = new StringContent("\"Aceite\"", Encoding.UTF8, "application/json");
            await client.PutAsync($"api/Convite/{id}/estado", content);
            return RedirectToPage();
        }*/

        public async Task<IActionResult> OnPostAceitarAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"api/Convite/{id}/aceitar", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Convite aceite com sucesso!";
            }
            else
            {
                TempData["Mensagem"] = "Erro ao aceitar o convite.";
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostRecusarAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var content = new StringContent("\"Recusado\"", Encoding.UTF8, "application/json");
            await client.PutAsync($"api/Convite/{id}/estado", content);
            return RedirectToPage();
        }
    }
}
