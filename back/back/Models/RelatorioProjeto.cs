using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("relatorio_projeto")]
public class RelatorioProjeto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    [Required]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Required]
    [Column("total_horas")]
    public int TotalHoras { get; set; }

    [Required]
    [Column("total_preco")]
    public decimal TotalPreco { get; set; }

}
