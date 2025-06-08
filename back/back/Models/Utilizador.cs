using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTesteESII.Models
{
    [Table("utilizador")]
    public class Utilizador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int HorasDia { get; set; } // ← já existe!

        [Required]
        [Column("tipo")]
        public string Tipo { get; set; } = "User";

        [Required]
        [Column("is_admin")]
        public bool IsAdmin { get; set; } = false;

        public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";

        public ICollection<UtilizadorProjeto>? UtilizadorProjetos { get; set; }

        public virtual void ExibirInformacoes()
        {
            Console.WriteLine($"{Nome} ({Email}) - {HorasDia} horas/dia");
        }

        public virtual bool PodeGerirUtilizadores()
        {
            return Tipo == "Admin" || Tipo == "UserManager";
        }
    }
}
