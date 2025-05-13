
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

                var response = await client.GetAsync("api/Projeto");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar projetos: {ex.Message}");
            }

            TotalProjetos = Projetos.Count;
            TarefasPendentes = Projetos.Count(p => p.Status == "Pendente");
            TotalClientes = Projetos.Select(p => p.Cliente).Distinct().Count();
        }

        public class Projeto
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cliente { get; set; }
            public string? Estado { get; set; }
        }

        public class UserInfoDTO
        {
            public string Nome { get; set; } = "";
            public string Email { get; set; } = "";
            public string Role { get; set; } = "";
        }
    }
}
