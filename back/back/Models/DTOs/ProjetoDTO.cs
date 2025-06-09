namespace BackendTesteESII.Models.DTOs
{
    public class ProjetoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;

    public int ClienteId { get; set; }   // ✅ Adiciona este campo!
    public decimal? PrecoHora { get; set; }

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int HorasTrabalho { get; set; }
}

}
