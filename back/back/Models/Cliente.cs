using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefone { get; set; }

    public virtual ICollection<Projeto1> Projeto1s { get; set; } = new List<Projeto1>();

    public virtual ICollection<Relatorioproj> Relatorioprojs { get; set; } = new List<Relatorioproj>();
}
