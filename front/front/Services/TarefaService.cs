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
    }
}
