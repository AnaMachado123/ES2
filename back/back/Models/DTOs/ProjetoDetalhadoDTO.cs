public class ProjetoDetalhadoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int HorasTrabalho { get; set; }

    public string NomeCliente { get; set; } = string.Empty;
    public string NomeCriador { get; set; } = string.Empty;

    public List<TarefaHistoricoDTO> Tarefas { get; set; } = new();
}
