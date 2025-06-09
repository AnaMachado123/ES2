using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace front.Pages.Projetos
{
    public class ConvidarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConvidarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int ProjetoId { get; set; }

        public List<UtilizadorDTO> Utilizadores { get; set; } = new();
        //public string? Mensagem { get; set; }

        [TempData]
        public string? MensagemConvite { get; set; }


        public class UtilizadorDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.GetAsync("api/Utilizador");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Utilizadores = JsonSerializer.Deserialize<List<UtilizadorDTO>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
        }

        public async Task<IActionResult> OnPostAsync(int utilizadorId)
        {
            var client = _httpClientFactory.CreateClient("Backend");

            var convite = new { UtilizadorId = utilizadorId, ProjetoId = ProjetoId };
            var content = new StringContent(JsonSerializer.Serialize(convite), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Convite", content);

            if (response.IsSuccessStatusCode)
                MensagemConvite = "Convite enviado com sucesso!";
            else
                MensagemConvite = "Erro ao enviar convite.";

            //return RedirectToPage(new { ProjetoId = ProjetoId });
            return RedirectToPage(new { id = ProjetoId });
        }
    }
}
