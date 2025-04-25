using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class TarefasModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string TabSelecionada { get; set; } = "emcurso";

        public void OnGet()
        {
            // Aqui depois vamos carregar as tarefas do utilizador logado via API/backend
        }
    }
}
