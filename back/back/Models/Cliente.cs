using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("cliente")]
public class Cliente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("telefone")]
    [StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

}
