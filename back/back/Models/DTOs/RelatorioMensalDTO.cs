public class RelatorioMensalDTO
{
    public int Dia { get; set; }
    public int TotalHoras { get; set; }
    public decimal TotalPreco { get; set; }
    public bool ExcedeuLimite { get; set; }
    public string? NomeProjeto { get; set; }
}
