using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models;

[Table("utilizador_projeto")]
public class UtilizadorProjeto
{
    [Column("utilizador_id")]
    public int UtilizadorId { get; set; }

    public Utilizador Utilizador { get; set; } = null!;

    [Column("projeto_id")]
    public int ProjetoId { get; set; }

    public Projeto Projeto { get; set; } = null!;
}
