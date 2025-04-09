using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front.Models;

namespace front.Pages
{
    public class PerfilEditarModel : PageModel
    {
        [BindProperty]
        public UtilizadorModel Utilizador { get; set; } = new();

        public void OnGet()
        {
            Utilizador = new UtilizadorModel
            {
                Nome = "Ana Machado",
                Email = "ana@email.com",
                CargaHorariaDiaria = 8
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("/PerfilVisualizar");
        }
    }
}
