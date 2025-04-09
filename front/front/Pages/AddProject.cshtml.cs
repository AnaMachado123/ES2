using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class AddProjectModel : PageModel
    {
        [BindProperty]
        public string? Nome { get; set; }

        [BindProperty]
        public decimal Precohora { get; set; }

        [BindProperty]
        public int Clienteid { get; set; }

        [BindProperty]
        public int Utilizadorid { get; set; }

        public string? Mensagem { get; set; }

        public void OnGet()
        {
            // Página carregada normalmente
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Clienteid <= 0 || Utilizadorid <= 0)
            {
                Mensagem = "Por favor preencha todos os campos obrigatórios.";
                return Page();
            }

            // Aqui no futuro vais chamar a API via HttpClient ou usar serviço para guardar os dados
            // Exemplo simulado:
            Console.WriteLine($"Projeto: {Nome}, Preço/Hora: {Precohora}, Cliente: {Clienteid}, Utilizador: {Utilizadorid}");

            // (Opcional) Pega as tarefas do form
            var form = Request.Form;
            var tarefas = new List<string>();
            foreach (var key in form.Keys)
            {
                if (key.Contains("descricao"))
                {
                    tarefas.Add(form[key]);
                }
            }

            Mensagem = "Projeto criado com sucesso!";

            return Page(); // Renderiza de novo a página com a mensagem
        }
    }
}
