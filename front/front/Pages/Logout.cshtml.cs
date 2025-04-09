using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace front.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Login", new { logout = true });
        }

        public IActionResult OnGet()
        {
            // Evita erro 400 se abrirem /Logout diretamente
            return RedirectToPage("/Login");
        }
    }
}
