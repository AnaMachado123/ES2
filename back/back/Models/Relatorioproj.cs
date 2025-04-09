using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Relatorioproj
{
    public int Id { get; set; }

    public decimal? Precotarefas { get; set; }

    public decimal? Precotot { get; set; }

    public int? Projetoid { get; set; }

    public int? Clientid { get; set; }

    public virtual Cliente? Client { get; set; }

    public virtual Projeto1? Projeto { get; set; }

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
