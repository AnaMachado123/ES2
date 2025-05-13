using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace front.Pages.Projetos
{
    public class EditarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ProjetoModel Projeto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.GetAsync($"api/Projeto/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();

            var projeto = JsonSerializer.Deserialize<ProjetoModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (projeto == null)
                return NotFound();

            Projeto = projeto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Projeto.Estado == "Concluído" && Projeto.DataFim == null)
            {
                ModelState.AddModelError("Projeto.DataFim", "A data de fim é obrigatória se o estado for 'Concluído'.");
            }

            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("Backend");

            var content = new StringContent(
                JsonSerializer.Serialize(Projeto),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync($"api/Projeto/{Projeto.Id}", content);

            if (!response.IsSuccessStatusCode)
                return Page(); // ou mostrar erro

            return RedirectToPage("/Projetos/Index");
        }

        public class ProjetoModel
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = string.Empty;
            public DateTime DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int HorasTrabalho { get; set; }
        }
    }
}
