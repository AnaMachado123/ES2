namespace BackendTesteESII.Models.DTOs
{
    public class RelatorioProjetoDTO
    {
        public string UtilizadorNome { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int Horas { get; set; }
        public decimal Preco { get; set; }
    }
}
