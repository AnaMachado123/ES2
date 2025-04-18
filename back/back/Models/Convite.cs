using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("convite")]
public class Convite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("utilizador_id")]
    public int UtilizadorId { get; set; }

    [Required]
    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    [Required]
    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = string.Empty;
}
