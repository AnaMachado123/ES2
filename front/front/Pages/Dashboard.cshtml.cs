using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace front.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string NomeUtilizador { get; set; } = "";
        public string EmailUtilizador { get; set; } = "";
        public string RoleUtilizador { get; set; } = "";
        public string TipoUtilizador { get; set; } = "regular";
        public string ModoVisualizacao { get; set; } = "todos";

        public int TotalProjetos { get; set; }
        public int TarefasPendentes { get; set; } = 0;
        public int TotalClientes { get; set; } = 0;

        public List<Projeto> Projetos { get; set; } = new();

        public async Task OnGetAsync(string? modo)
        {
            TipoUtilizador = HttpContext.Session.GetString("Tipo") ?? "regular";
            ModoVisualizacao = modo ?? "todos";

            var client = _httpClientFactory.CreateClient("Backend");

            try
            {
                var token = HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Buscar info do utilizador autenticado
                var userResponse = await client.GetAsync("api/utilizador/me");
                if (userResponse.IsSuccessStatusCode)
                {
                    var json = await userResponse.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<UserInfoDTO>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (userInfo != null)
                    {
                        NomeUtilizador = userInfo.Nome;
                        EmailUtilizador = userInfo.Email;
                        RoleUtilizador = userInfo.Role;
                    }
                }

                // 🔄 Escolher endpoint com base no modo e role
                var endpointProjetos = (ModoVisualizacao == "todos" && RoleUtilizador == "Admin")
                    ? "api/projeto/todos"
                    : "api/projeto";

                var response = await client.GetAsync(endpointProjetos);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var projetosApi = JsonSerializer.Deserialize<List<Projeto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (projetosApi != null)
                    {
                        Projetos = projetosApi;
                    }
                }

                // ✅ Corrigido: passar 'modo' ao endpoint de contagem
                var clientesResponse = await client.GetAsync($"api/projeto/clientes/contagem?modo={ModoVisualizacao}");
                if (clientesResponse.IsSuccessStatusCode)
                {
                    var json = await clientesResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    TotalClientes = doc.RootElement.GetProperty("totalClientes").GetInt32();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dados do dashboard: {ex.Message}");
            }

            TotalProjetos = Projetos.Count;
            TarefasPendentes = Projetos.Count(p => p.Estado == "Pendente");
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; } = "";
            public int ClienteId { get; set; }
            public string? Estado { get; set; } = "Indefinido";
            public string? Status => Estado;
        }

        public class UserInfoDTO
        {
            public string Nome { get; set; } = "";
            public string Email { get; set; } = "";
            public string Role { get; set; } = "";
        }
    }
}
