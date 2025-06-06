using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        [BindProperty] public decimal Precohora { get; set; }
        [BindProperty] public string NomeCliente { get; set; } = string.Empty;
        [BindProperty] public string Descricao { get; set; } = string.Empty;
        [BindProperty] public DateTime DataInicio { get; set; } = DateTime.Today;
        [BindProperty] public DateTime DataFim { get; set; } = DateTime.Today.AddDays(30);
        [BindProperty] public int HorasTrabalho { get; set; } = 40;

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

            if (Request.Cookies.TryGetValue("jwt", out string? jwt))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            }

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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Backend");

            if (!Request.Cookies.TryGetValue("jwt", out string? jwt))
            {
                Mensagem = "Token não encontrado. Por favor, faça login novamente.";
                return RedirectToPage("/Login");
            }

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            // Obter clienteId real da API
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

            // Tarefas
            var descricoes = Request.Form["descricao"];
            var datasInicio = Request.Form["dataInicio"];
            var datasFim = Request.Form["dataFim"];

            var tarefas = new List<object>();

            int numTarefas = Math.Min(descricoes.Count, Math.Min(datasInicio.Count, datasFim.Count));

            for (int i = 0; i < numTarefas; i++)
            {
                if (string.IsNullOrWhiteSpace(descricoes[i]) ||
                    string.IsNullOrWhiteSpace(datasInicio[i]) ||
                    string.IsNullOrWhiteSpace(datasFim[i]))
                    continue;

               tarefas.Add(new
                {
                    Descricao = descricoes[i],
                    DataInicio = DateTime.Parse(datasInicio[i]),
                    DataFim = DateTime.Parse(datasFim[i]),
                    Status = "Em curso",
                    HorasGastas = 0,
                    UtilizadorId = 0
                });

            }

            // Projeto
            var projeto = new
            {
                nome = Nome,
                descricao = Descricao,
                dataInicio = DataInicio,
                dataFim = DataFim,
                clienteId = cliente.Id,
                horasTrabalho = HorasTrabalho,
                utilizadorId = 0,
                estado = "Pendente",
                tarefas = tarefas
            };

            var content = new StringContent(JsonSerializer.Serialize(projeto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Projeto", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Projetos/Index");
            }

            Mensagem = "Erro ao criar projeto.";
            ClientesDisponiveis = clientes?.Select(c => c.Nome).ToList() ?? new();
            return Page();
        }
    }
}
