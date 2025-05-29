using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Login");

            int userId = int.Parse(userIdStr);

            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.GetAsync($"api/Convite/utilizador/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var convites = JsonSerializer.Deserialize<List<ConviteDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (convites != null)
                    Convites = convites.Where(c => c.Estado == "Pendente").ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAceitarAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var content = new StringContent("\"Aceite\"", Encoding.UTF8, "application/json");
            await client.PutAsync($"api/Convite/{id}/estado", content);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRecusarAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var content = new StringContent("\"Recusado\"", Encoding.UTF8, "application/json");
            await client.PutAsync($"api/Convite/{id}/estado", content);
            return RedirectToPage();
        }

        public class ConviteDTO
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = string.Empty;
        }
    }
}
