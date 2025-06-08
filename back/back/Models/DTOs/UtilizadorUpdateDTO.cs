namespace BackendTesteESII.Models.DTOs
{
    public class UtilizadorUpdateDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CargaHorariaDiaria { get; set; }
        public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";
    }
}
