using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string? Nome { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string TipoPerfil { get; set; } = "User"; // tipo aceito pelo backend

        public async Task<IActionResult> OnPostAsync()
        {
            var novoUtilizador = new
            {
                Nome = Nome,
                Email = Email,
                Password = Password,
                Tipo = TipoPerfil
            };

            var client = _httpClientFactory.CreateClient("Backend");

            var json = JsonSerializer.Serialize(novoUtilizador);
            Console.WriteLine("JSON ENVIADO:");
            Console.WriteLine(json); // 💥 Aqui vês o que vai para o backend

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Utilizador", content);
            var responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("RESPOSTA: " + responseText);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }

            ModelState.AddModelError(string.Empty, "Erro ao criar conta.");
            return Page();
        }
    }
}
