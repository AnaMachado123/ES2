public class TarefaHistoricoDTO
{
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Status { get; set; } = string.Empty;
    public int HorasGastas { get; set; }
    public string NomeUtilizador { get; set; } = string.Empty;
}
