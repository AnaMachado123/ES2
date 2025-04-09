using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string? Nome { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // Aqui só redirecionamos direto para o Login
            return RedirectToPage("/Login");
        }
    }
}
