using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("projeto")]
public class Projeto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("descricao")]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Column("data_inicio")]
    public DateTime DataInicio { get; set; }

    [Required]
    [Column("data_fim")]
    public DateTime DataFim { get; set; }

    [Required]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Required]
    [Column("utilizador_id")]
    public int UtilizadorId { get; set; }

    [Required]
    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = string.Empty;

    [Column("concluido")]
    public bool Concluido { get; set; } = false;

    [Required]
    [Column("preco_hora")]
    public int HorasTrabalho { get; set; }  // ← usado como preço por hora

    public ICollection<UtilizadorProjeto> UtilizadorProjetos { get; set; } = new List<UtilizadorProjeto>();
    public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
