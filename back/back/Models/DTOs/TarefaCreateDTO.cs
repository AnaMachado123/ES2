using System.ComponentModel.DataAnnotations;

namespace BackendTesteESII.Models.DTOs;

public class TarefaCreateDTO
{
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Status { get; set; } = "Em curso";
    public int HorasGastas { get; set; } = 0;
    public int UtilizadorId { get; set; }
}
