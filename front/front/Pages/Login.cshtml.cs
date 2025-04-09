using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Mensagem { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Verificação se os campos foram preenchidos
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Mensagem = "Por favor, preencha todos os campos.";
                return Page();
            }

            // Login sempre válido (simulação, aceitar qualquer email/senha)
            return RedirectToPage("/Dashboard");
        }
    }
}
