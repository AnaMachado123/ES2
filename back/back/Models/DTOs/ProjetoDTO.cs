namespace front.Models;

public class ProjetoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty; // ← Nome do cliente
}
