using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class AddUserModel : PageModel
    {
        private readonly ILogger<AddUserModel> _logger;

        public AddUserModel(ILogger<AddUserModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Lógica que quiseres executar quando a página for carregada
        }
    }
}
