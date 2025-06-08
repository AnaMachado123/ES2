namespace BackendTesteESII.Models.DTOs;

public class ConviteDTO
{
    public int Id { get; set; }
    public int ProjetoId { get; set; }
    public int UtilizadorId { get; set; }
    public string Estado { get; set; } = string.Empty;

    public string ProjetoNome { get; set; } = string.Empty;
}
