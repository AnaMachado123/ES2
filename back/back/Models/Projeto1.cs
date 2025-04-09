using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Projeto1
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int? Utilizadorid { get; set; }

    public int? Clienteid { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Convite> Convites { get; set; } = new List<Convite>();

    public virtual ICollection<Relatorioproj> Relatorioprojs { get; set; } = new List<Relatorioproj>();

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    public virtual Utilizador? Utilizador { get; set; }

    public virtual ICollection<Utilizador> Utilizadors { get; set; } = new List<Utilizador>();
}
