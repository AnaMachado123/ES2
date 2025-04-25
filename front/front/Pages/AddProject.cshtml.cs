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
        public List<string> ClientesDisponiveis { get; set; } = new();

        public string? Mensagem { get; set; }

        public void OnGet()
        {
            // Simulação: esta lista pode ser carregada da API no futuro
            ClientesDisponiveis = new List<string> { "Eduarda Gomes", "Adriana Meira", "Diana Matos" };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Simulação de correspondência entre nome e ID de cliente
            var mapaClientes = new Dictionary<string, int>
            {
                { "Eduarda Gomes", 5 },
                { "Adriana Meira", 6 },
                { "Diana Matos", 7 }
            };

            if (!mapaClientes.TryGetValue(NomeCliente, out int clienteId))
            {
                Mensagem = "Cliente inválido selecionado.";
                return Page();
            }

            var descricoes = Request.Form["descricao"];
            var datasInicio = Request.Form["dataInicio"];
            var datasFim = Request.Form["dataFim"];

            var tarefas = new List<object>();

            for (int i = 0; i < descricoes.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(descricoes[i]) || string.IsNullOrWhiteSpace(datasInicio[i]) || string.IsNullOrWhiteSpace(datasFim[i]))
                    continue;

                tarefas.Add(new
                {
                    descricao = descricoes[i],
                    dataInicio = DateTime.Parse(datasInicio[i]),
                    dataFim = DateTime.Parse(datasFim[i]),
                    status = "Em curso",
                    horasGastas = 0,
                    projetoId = 0,
                    utilizadorId = 0 // ⚠️ Removido: se quiseres podemos associar depois
                });
            }

            var projeto = new
            {
                nome = Nome,
                descricao = "Projeto criado pelo formulário",
                dataInicio = DateTime.UtcNow,
                dataFim = DateTime.UtcNow.AddMonths(1),
                clienteId = clienteId,
                horasTrabalho = 40,
                utilizadorId = 0,
                estado = "Pendente",
                tarefas = tarefas
            };

            var client = _httpClientFactory.CreateClient("Backend");
            var content = new StringContent(JsonSerializer.Serialize(projeto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Projeto", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Projetos/Index");
            }

            Mensagem = "Erro ao criar projeto.";
            return Page();
        }
    }
}
