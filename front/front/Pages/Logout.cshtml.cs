using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Apenas remove o token JWT
            Response.Cookies.Delete("jwt");
            HttpContext.Session.Clear();
            // Dados da sessão (nome, email, etc.) continuam guardados
            return RedirectToPage("/Login");
        }

        public IActionResult OnPost()
        {
            // Mesmo comportamento para segurança
            Response.Cookies.Delete("jwt");
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
