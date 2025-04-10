using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("utilizador")]
public class Utilizador
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
    [Column("password")]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("horas_dia")]
    public int HorasDia { get; set; }

    public ICollection<UtilizadorProjeto>? UtilizadorProjetos { get; set; }
    
    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; } = false;

}
