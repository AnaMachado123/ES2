using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace front.Pages.Projetos
{
    public class ApagarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApagarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ProjetoInfo Projeto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.GetAsync($"api/Projeto/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = "Erro ao buscar o projeto.";
                return RedirectToPage("/Projetos/Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            var projeto = JsonSerializer.Deserialize<ProjetoInfo>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (projeto == null)
            {
                TempData["MensagemSucesso"] = "Projeto não encontrado.";
                return RedirectToPage("/Projetos/Index");
            }

            Projeto = projeto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Backend");
            var response = await client.DeleteAsync($"api/Projeto/{Projeto.Id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = $"Projeto \"{Projeto.Nome}\" foi apagado com sucesso.";
            }
            else
            {
                TempData["MensagemSucesso"] = "Erro ao apagar o projeto.";
            }

            return RedirectToPage("/Projetos/Index");
        }

        public class ProjetoInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Cliente { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }
    }
}
