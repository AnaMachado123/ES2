namespace front.Models
{

    public class ProjetoDetalhadoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Estado { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal HorasTrabalho { get; set; }
        public string? NomeCliente { get; set; }
        public string? NomeCriador { get; set; }
        public List<TarefaHistoricoDTO> Tarefas { get; set; } = new();
    }

    public class TarefaHistoricoDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int HorasGastas { get; set; }
    }

    public class MembroDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

