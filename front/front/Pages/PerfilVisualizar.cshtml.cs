using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace front.Pages
{
    public class PerfilVisualizarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PerfilVisualizarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public int CargaHoraria { get; set; }
        public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";

        public int ProjetosAtivos { get; set; }
        public int TarefasEmCurso { get; set; }
        public int HorasEsteMes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("jwt", out var jwt))
                return RedirectToPage("/Login");

            var client = _httpClientFactory.CreateClient("Backend");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            // Obter info do utilizador
            var userInfoResponse = await client.GetAsync("api/Utilizador/me");
            if (!userInfoResponse.IsSuccessStatusCode)
                return RedirectToPage("/Login");

            var jsonUser = await userInfoResponse.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserInfoDTO>(jsonUser, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Nome = user?.Nome ?? "";
            Email = user?.Email ?? "";
            CargaHoraria = user?.CargaHorariaDiaria ?? 8;
            ImagemPerfil = user?.ImagemPerfil ?? "/images/default-profile.jpg";

            // Obter resumo
            var resumoResponse = await client.GetAsync("api/Resumo");
            if (resumoResponse.IsSuccessStatusCode)
            {
                var jsonResumo = await resumoResponse.Content.ReadAsStringAsync();
                var resumo = JsonSerializer.Deserialize<ResumoDTO>(jsonResumo, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ProjetosAtivos = resumo?.ProjetosAtivos ?? 0;
                TarefasEmCurso = resumo?.TarefasEmCurso ?? 0;
                HorasEsteMes = resumo?.HorasGastasEsteMes ?? 0;
            }

            return Page();
        }

        public class UserInfoDTO
        {
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public int CargaHorariaDiaria { get; set; }
            public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";
        }

        public class ResumoDTO
        {
            public int ProjetosAtivos { get; set; }
            public int TarefasEmCurso { get; set; }
            public int HorasGastasEsteMes { get; set; }
        }
    }
}
