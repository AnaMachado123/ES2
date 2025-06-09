using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;

namespace front.Pages
{
    public class AddProjectModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddProjectModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty] public string Nome { get; set; } = string.Empty;
        [BindProperty] public int HorasTrabalho { get; set; }
        [BindProperty] public string NomeCliente { get; set; } = string.Empty;
        [BindProperty] public string Descricao { get; set; } = string.Empty;
        [BindProperty] public DateTime DataInicio { get; set; } = DateTime.Today;
        [BindProperty] public DateTime DataFim { get; set; } = DateTime.Today.AddDays(30);

        public List<string> ClientesDisponiveis { get; set; } = new();
        public string? Mensagem { get; set; }

        public class ClienteDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; } = "";
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Backend");

            if (!Request.Cookies.TryGetValue("jwt", out string? jwt))
            {
                Mensagem = "Token não encontrado. Faça login para continuar.";
                return;
            }

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("api/Cliente");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var clientes = JsonSerializer.Deserialize<List<ClienteDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ClientesDisponiveis = clientes?.Select(c => c.Nome).ToList() ?? new();
            }
            else
            {
                Mensagem = "Erro ao carregar clientes.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Backend");

            var hasCookie = Request.Cookies.TryGetValue("jwt", out string? jwt);
            Console.WriteLine($"JWT encontrado: {hasCookie}, valor: {jwt}");

            if (!hasCookie || string.IsNullOrEmpty(jwt))
            {
                Mensagem = "Você não está autenticado. Faça login e tente novamente.";
                ClientesDisponiveis = new();
                return Page();
            }

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var userIdClaim = token.Claims.FirstOrDefault(c =>
                c.Type == "nameid" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var userIdStr = userIdClaim?.Value;

            if (!int.TryParse(userIdStr, out int userId))
            {
                Mensagem = "ID do utilizador inválido. Por favor, refaça o login.";
                ClientesDisponiveis = new();
                return Page();
            }

            var responseClientes = await client.GetAsync("api/Cliente");
            var jsonClientes = await responseClientes.Content.ReadAsStringAsync();
            var clientes = JsonSerializer.Deserialize<List<ClienteDTO>>(jsonClientes, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var cliente = clientes?.FirstOrDefault(c => c.Nome == NomeCliente);
            if (cliente == null)
            {
                Mensagem = "Cliente inválido selecionado.";
                ClientesDisponiveis = clientes?.Select(c => c.Nome).ToList() ?? new();
                return Page();
            }

            var descricoes = Request.Form["descricao"];
            var datasInicio = Request.Form["dataInicio"];
            var datasFim = Request.Form["dataFim"];
            var statusList = Request.Form["status"];
            var horasGastasList = Request.Form["horasGastas"];

            var tarefas = new List<object>();
            int numTarefas = new[] { descricoes.Count, datasInicio.Count, datasFim.Count, statusList.Count, horasGastasList.Count }.Min();

            Console.WriteLine("== TAREFAS RECOLHIDAS DO FORMULARIO ==");
            for (int i = 0; i < numTarefas; i++)
            {
                Console.WriteLine($"[{i}] descricao={descricoes[i]}, dataInicio={datasInicio[i]}, dataFim={datasFim[i]}, status={statusList[i]}, horas={horasGastasList[i]}");
            }

            for (int i = 0; i < numTarefas; i++)
            {
                if (string.IsNullOrWhiteSpace(descricoes[i]) ||
                    string.IsNullOrWhiteSpace(datasInicio[i]) ||
                    string.IsNullOrWhiteSpace(datasFim[i]) ||
                    string.IsNullOrWhiteSpace(statusList[i]) ||
                    string.IsNullOrWhiteSpace(horasGastasList[i]))
                    continue;

                if (!DateTime.TryParse(datasInicio[i], out var inicio) ||
                    !DateTime.TryParse(datasFim[i], out var fim) ||
                    !int.TryParse(horasGastasList[i], out var horas))
                    continue;

                tarefas.Add(new
                {
                    Descricao = descricoes[i],
                    DataInicio = inicio.ToUniversalTime(),
                    DataFim = fim.ToUniversalTime(),
                    Status = statusList[i],
                    HorasGastas = horas,
                    UtilizadorId = userId
                });
            }

            var projeto = new
            {
                Nome = Nome,
                Descricao = Descricao,
                DataInicio = DataInicio.ToUniversalTime(),
                DataFim = DataFim.ToUniversalTime(),
                ClienteId = cliente.Id,
                HorasTrabalho = HorasTrabalho,
                UtilizadorId = userId,
                Estado = "Pendente",
                Tarefas = tarefas
            };

            Console.WriteLine("== JSON FINAL ENVIADO ==");
            Console.WriteLine(JsonSerializer.Serialize(projeto, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = null
            }));

            var content = new StringContent(
                JsonSerializer.Serialize(projeto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = false
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("api/Projeto", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Projetos/Index");
            }

            var erro = await response.Content.ReadAsStringAsync();
            Mensagem = $"Erro ao criar projeto: {erro}";
            ClientesDisponiveis = clientes?.Select(c => c.Nome).ToList() ?? new();
            return Page();
        }
    }
}
