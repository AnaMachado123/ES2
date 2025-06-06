public class Tarefa
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Projeto { get; set; } // ✨ novo campo
    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public bool Concluida => Fim.HasValue;
}
