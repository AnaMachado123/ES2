using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("relatorio")]
public class Relatorio
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("utilizador_id")]
    public int UtilizadorId { get; set; }

    [Required]
    [Column("mes")]
    public int Mes { get; set; }

    [Required]
    [Column("ano")]
    public int Ano { get; set; }

    [Required]
    [Column("total_horas")]
    public int TotalHoras { get; set; }

    [Required]
    [Column("total_preco")]
    public decimal TotalPreco { get; set; }
}
