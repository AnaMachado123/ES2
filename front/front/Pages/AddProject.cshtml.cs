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
            // Carregamento inicial
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Clienteid <= 0 || Utilizadorid <= 0)
            {
                Mensagem = "Por favor preencha todos os campos obrigatórios.";
                return Page();
            }

            // Aqui futuramente vais guardar na base de dados (API/backend)
            Console.WriteLine($"Projeto Criado: {Nome}, Preço/Hora: {Precohora}, ClienteID: {Clienteid}, UtilizadorID: {Utilizadorid}");

            TempData["MensagemSucesso"] = $"Projeto \"{Nome}\" criado com sucesso!";
            return RedirectToPage("/Projetos/Index");
        }
    }
}
