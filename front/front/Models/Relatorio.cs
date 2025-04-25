namespace front.Models;

public class Relatorio
{
    public int Id { get; set; }
    public int UtilizadorId { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int TotalHoras { get; set; }
    public decimal TotalPreco { get; set; }
    public string? Texto { get; set; }
}
