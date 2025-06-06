using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

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

        public List<MembroDTO> Membros { get; set; } = new();
        public List<ClienteDTO> Clientes { get; set; } = new();

        private async Task LoadDadosAsync(int id, string token)
        {
            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Projeto
            var response = await client.GetAsync($"api/Projeto/{id}/detalhado");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var projetoApi = JsonSerializer.Deserialize<ProjetoDetalhadoDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (projetoApi != null)
                {
                    Projeto = new ProjetoModel
                    {
                        Id = id,
                        Nome = projetoApi.Nome,
                        Descricao = projetoApi.Descricao,
                        ClienteId = projetoApi.ClienteId,
                        UtilizadorId = projetoApi.UtilizadorId,
                        Estado = projetoApi.Estado,
                        DataInicio = projetoApi.DataInicio,
                        DataFim = projetoApi.DataFim,
                        HorasTrabalho = projetoApi.HorasTrabalho
                    };
                }
            }

            // Membros
            var membrosResp = await client.GetAsync($"api/Projeto/{id}/membros");
            if (membrosResp.IsSuccessStatusCode)
            {
                var json = await membrosResp.Content.ReadAsStringAsync();
                Membros = JsonSerializer.Deserialize<List<MembroDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }

            // Clientes
            var clientesResp = await client.GetAsync("api/Cliente");
            if (clientesResp.IsSuccessStatusCode)
            {
                var json = await clientesResp.Content.ReadAsStringAsync();
                Clientes = JsonSerializer.Deserialize<List<ClienteDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            await LoadDadosAsync(id, token);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            // ✅ Força o UtilizadorId com o ID da sessão (caso não esteja presente)
            var userId = HttpContext.Session.GetInt32("UtilizadorId") ?? 0;
            if (Projeto.UtilizadorId == 0)
            {
                Projeto.UtilizadorId = userId;
            }

            if (Projeto.Estado == "Concluído" && Projeto.DataFim == null)
            {
                ModelState.AddModelError("Projeto.DataFim", "A data de fim é obrigatória se o estado for 'Concluído'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDadosAsync(Projeto.Id, token);
                return Page();
            }

            Console.WriteLine("🧩 ENVIANDO PROJETO:");
            Console.WriteLine($"Id: {Projeto.Id}");
            Console.WriteLine($"Nome: {Projeto.Nome}");
            Console.WriteLine($"ClienteId: {Projeto.ClienteId}");
            Console.WriteLine($"UtilizadorId: {Projeto.UtilizadorId}");
            Console.WriteLine($"Estado: {Projeto.Estado}");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(Projeto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Projeto/{Projeto.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                await LoadDadosAsync(Projeto.Id, token);
                return Page();
            }

            return RedirectToPage("/Projetos/Index");
        }


        public async Task<IActionResult> OnPostRemoverMembroAsync(int utilizadorId)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"api/Convite/remover?utilizadorId={utilizadorId}&projetoId={Projeto.Id}");

            await LoadDadosAsync(Projeto.Id, token);
            return Page();
        }

        public class ProjetoModel
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
            public string Descricao { get; set; } = "";
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; } = "";
            public DateTime DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int HorasTrabalho { get; set; }
        }

        public class ProjetoDetalhadoDTO
        {
            public string Nome { get; set; } = "";
            public string Descricao { get; set; } = "";
            public string Estado { get; set; } = "";
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public int HorasTrabalho { get; set; }
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
        }

        public class MembroDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public class ClienteDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
        }
    }
}
