using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginData = new
            {
                Email = Email,
                Password = Password
            };

            var client = _httpClientFactory.CreateClient("Backend");

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

           var response = await client.PostAsync("api/Login", content);


            if (response.IsSuccessStatusCode)
            {
                // (Opcional) Ler a resposta com os dados do utilizador logado
                var responseBody = await response.Content.ReadAsStringAsync();

                var utilizador = JsonSerializer.Deserialize<UtilizadorResponse>(responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Guardar em sessão (exemplo)
                if (utilizador != null)
                {
                    HttpContext.Session.SetString("Nome", utilizador.Nome ?? "");
                    HttpContext.Session.SetString("Email", utilizador.Email ?? "");
                    HttpContext.Session.SetString("Tipo", utilizador.Tipo ?? "User");
                }

                return RedirectToPage("/Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Login inválido. Verifica o email e a senha.");
            return Page();
        }

        // Classe auxiliar para ler o JSON de resposta
        public class UtilizadorResponse
        {
            public string? Nome { get; set; }
            public string? Email { get; set; }
            public string? Tipo { get; set; } // pode ser "Admin", "UserManager", etc.
        }
    }
}
