using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class EditarModel : PageModel
    {
        [BindProperty]
        public ProjetoEditavel Projeto { get; set; }

        public void OnGet(int id)
        {
            // Simula buscar dados do projeto (por enquanto estático)
            Projeto = new ProjetoEditavel
            {
                Id = id,
                Nome = "Gestão de Redes",
                Descricao = "Sistema de Gestão de Redes desenvolvido para monitorizar, administrar e otimizar a infraestrutura de rede de uma organização. Permite acompanhar o desempenho de dispositivos, gerir permissões de acesso, detetar falhas em tempo real e gerar relatórios detalhados. A solução oferece integração com dashboards interativos, suporte a protocolos de segurança e funcionalidades de manutenção preventiva, garantindo maior fiabilidade, eficiência e segurança em ambientes empresariais ou académicos.",
                DataInicio = new DateTime(2025, 1, 15),
                Estado = "Em Curso",
                ClienteId = 101,
                UtilizadorId = 99
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Simular salvamento
            // Aqui você colocaria lógica para salvar no banco

            return RedirectToPage("/Projetos/Ver", new { id = Projeto.Id });
        }

        public class ProjetoEditavel
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public DateTime DataInicio { get; set; }
            public string Estado { get; set; }
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
        }
    }
}
