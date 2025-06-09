using System.Net.Http;
using System.Net.Http.Json;
using front.Models;

namespace front.Services
{
    public class TarefaService
    {
        private readonly HttpClient _http;

        public TarefaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Tarefa>> GetTarefasEmCursoAsync(int utilizadorId)
        {
            return await _http.GetFromJsonAsync<List<Tarefa>>($"/api/tarefa/emcurso?utilizadorId={utilizadorId}") ?? new();
        }

        public async Task<List<Tarefa>> GetTarefasFinalizadasAsync(int utilizadorId)
        {
            return await _http.GetFromJsonAsync<List<Tarefa>>($"/api/tarefa/finalizadas?utilizadorId={utilizadorId}") ?? new();
        }

        public async Task<List<Tarefa>> GetTarefasIndividuaisAsync(int utilizadorId)
        {
            var tarefas = await _http.GetFromJsonAsync<List<Tarefa>>($"/api/tarefa/emcurso?utilizadorId={utilizadorId}");
            return tarefas?.Where(t => t.ProjetoId == null).ToList() ?? new();
        }

        public async Task<List<Projeto>> GetProjetosAsync()
        {
            return await _http.GetFromJsonAsync<List<Projeto>>("/api/projeto") ?? new();
        }

        public async Task<bool> CreateTarefaAsync(Tarefa nova)
        {
            Console.WriteLine("🚀 Enviando tarefa:");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nova));

            var response = await _http.PostAsJsonAsync("/api/tarefa", nova);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro ao criar tarefa: {response.StatusCode} → {erro}");
            }
            else
            {
                Console.WriteLine("✅ Tarefa criada com sucesso.");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> FinalizarTarefaAsync(int tarefaId)
        {
            var response = await _http.PutAsync($"/api/tarefa/{tarefaId}/finalizar", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverTarefaAsync(int tarefaId)
        {
            var response = await _http.DeleteAsync($"/api/tarefa/{tarefaId}");
            return response.IsSuccessStatusCode;
        }
    }
}
