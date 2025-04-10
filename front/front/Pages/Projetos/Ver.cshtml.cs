using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages.Projetos
{
    public class VerModel : PageModel
    {
        [BindProperty]
        public ProjetoInfo Projeto { get; set; }

        public void OnGet(int id)
        {
            Projeto = new ProjetoInfo
            {
                Id = id,
                Nome = "Gestão de Redes",
                Cliente = "Eduarda Gomes",
                ClienteId = 101,
                UtilizadorId = 99,
                Estado = "Em Curso",
                Descricao = "Sistema de Gestão de Redes desenvolvido para monitorizar, administrar e otimizar a infraestrutura de rede de uma organização. Permite acompanhar o desempenho de dispositivos, gerir permissões de acesso, detetar falhas em tempo real e gerar relatórios detalhados. A solução oferece integração com dashboards interativos, suporte a protocolos de segurança e funcionalidades de manutenção preventiva, garantindo maior fiabilidade, eficiência e segurança em ambientes empresariais ou académicos.",
                DataInicio = new DateTime(2025, 1, 15),
                DataFim = new DateTime(2025, 3, 1),
                HorasTrabalho = 140
            };
        }

        public class ProjetoInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cliente { get; set; }
            public int ClienteId { get; set; }
            public int UtilizadorId { get; set; }
            public string Estado { get; set; }
            public string Descricao { get; set; }
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
            public int HorasTrabalho { get; set; }
        }
    }
}