using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace front.Pages
{
    public class PerfilEditarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PerfilEditarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UtilizadorDTO Utilizador { get; set; } = new UtilizadorDTO();

        public class UtilizadorDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
            public string Email { get; set; } = "";
            public int CargaHorariaDiaria { get; set; }
            public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine("⚙️ Carregando perfil para edição...");

            if (!Request.Cookies.TryGetValue("jwt", out var jwt))
                return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("api/Utilizador/me");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Erro ao carregar perfil. Status: {response.StatusCode}");
                return RedirectToPage("/Login");
            }

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UtilizadorDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (user == null)
            {
                Console.WriteLine("❌ Utilizador não encontrado na resposta.");
                return RedirectToPage("/Login");
            }

            Utilizador = user;

            Console.WriteLine($"✅ Dados carregados: {user.Nome}, {user.Email}, {user.CargaHorariaDiaria}h/dia, ID: {user.Id}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("📨 Iniciando envio do perfil atualizado...");
            Console.WriteLine($"➡️ Enviando PUT para api/Utilizador/{Utilizador.Id}");

            if (!Request.Cookies.TryGetValue("jwt", out var jwt))
                return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var json = JsonSerializer.Serialize(Utilizador);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Utilizador/{Utilizador.Id}", content);

            Console.WriteLine($"📬 PUT status code: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/PerfilVisualizar");

            ModelState.AddModelError("", "Erro ao guardar alterações.");
            return Page();
        }
    }
}
