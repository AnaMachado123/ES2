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
        private string responseText = string.Empty; // ⚠️ Agora inicializado!

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

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Montar objeto com os dados do novo utilizador
            var novoUtilizador = new
            {
                Nome = Nome,
                Email = Email,
                Password = Password,
                Tipo = "regular" // ou "admin" se quiseres testar admin
            };

            var client = _httpClientFactory.CreateClient("Backend");

            var json = JsonSerializer.Serialize(novoUtilizador);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Utilizador", content);

            // 👇 Captura o conteúdo da resposta (para debug)
            responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("RESPONSE: " + responseText);

            if (response.IsSuccessStatusCode)
            {
                // Redirecionar para login se sucesso
                return RedirectToPage("/Login");
            }

            // Se deu erro, mostrar mensagem
            ModelState.AddModelError(string.Empty, "Erro ao criar conta.");
            return Page();
        }
    }
}
