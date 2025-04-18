using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

public class ProjetoCreateDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int ClienteId { get; set; }
    public int HorasTrabalho { get; set; }
    public int UtilizadorId { get; set; }
    public string Estado { get; set; } = string.Empty;
}
