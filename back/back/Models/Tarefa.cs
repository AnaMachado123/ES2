using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendTesteESII.Models;

[Table("tarefa")]
public class Tarefa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("descricao")]
    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Column("data_inicio")]
    public DateTime DataInicio { get; set; }

    [Required]
    [Column("data_fim")]
    public DateTime DataFim { get; set; }

    // 🔁 Atualizado: removido [Required]
    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [Column("projeto_id")]
    public int? ProjetoId { get; set; }

    [ForeignKey("ProjetoId")]
    [JsonIgnore]
    public Projeto? Projeto { get; set; }

    [Required]
    [Column("utilizador_id")]
    public int UtilizadorId { get; set; }

    [Required]
    [Column("horas_gastas")]
    public int HorasGastas { get; set; }

    [ForeignKey("UtilizadorId")]
    public Utilizador? Utilizador { get; set; }
}
