public class Tarefa
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Projeto { get; set; } // ✨ novo campo
    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public bool Concluida => Fim.HasValue;

    public static List<Tarefa> Exemplo() => new()
    {
        new Tarefa { Titulo = "Configurar Router", Projeto = "Gestão de Redes", Descricao = "Configuração de equipamentos de rede", Inicio = DateTime.Today.AddHours(9) },
        new Tarefa { Titulo = "Design Homepage", Projeto = "Website IPVC", Descricao = "Layout inicial no Figma", Inicio = DateTime.Today.AddHours(10.5) },
        new Tarefa { Titulo = "Criação de API GET", Projeto = "API Financeira", Descricao = "Criação do endpoint para consulta de transações", Inicio = DateTime.Today.AddHours(11.75) },
        new Tarefa { Titulo = "Login JWT", Projeto = "Website IPVC", Descricao = "Implementar autenticação segura", Inicio = DateTime.Today.AddHours(13), Fim = DateTime.Today.AddHours(15.5) },
        new Tarefa { Titulo = "Base de Dados Clientes", Projeto = "API Financeira", Descricao = "Estruturação da base de dados em EF Core", Inicio = DateTime.Today.AddHours(14), Fim = DateTime.Today.AddHours(16.25) }
    };
}
