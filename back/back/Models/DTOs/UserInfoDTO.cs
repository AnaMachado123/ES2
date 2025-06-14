namespace BackendTesteESII.Models.DTOs
{
    public class UserInfoDTO
    {
        public int Id { get; set; }  // ← agora incluído para uso no PUT
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int CargaHorariaDiaria { get; set; }
        public string ImagemPerfil { get; set; } = "/images/default-profile.jpg";

    }
}
