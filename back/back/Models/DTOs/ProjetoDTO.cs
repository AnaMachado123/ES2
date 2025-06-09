namespace BackendTesteESII.Models.DTOs
{
    public class ProjetoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public int HorasTrabalho { get; set; }  // ← representa o preço por hora
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
         public int ClienteId { get; set; }
    }
}
