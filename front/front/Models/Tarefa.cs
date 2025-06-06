using System;

namespace front.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; }
        public int ProjetoId { get; set; }
        public int UtilizadorId { get; set; }
        public int HorasGastas { get; set; }

        // Propriedades auxiliares para apresentação (opcional)
        public string Estado => Status?.ToLower() == "finalizada" ? "Concluída" : "Em curso";
        public bool Concluida => Status?.ToLower() == "finalizada";
    }
}