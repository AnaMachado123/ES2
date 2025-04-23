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
        private string responseText = string.Empty;

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
        public string Tipo { get; set; } = "regular"; // Novo campo!

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var novoUtilizador = new
            {
                Nome = Nome,
                Email = Email,
                Password = Password,
                Tipo = Tipo // Usa o valor escolhido no formulário
            };

            var client = _httpClientFactory.CreateClient("Backend");

            var json = JsonSerializer.Serialize(novoUtilizador);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Utilizador", content);

            responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("RESPONSE: " + responseText);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }

            ModelState.AddModelError(string.Empty, "Erro ao criar conta.");
            return Page();
        }
    }
}
