using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Tarefa
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public DateOnly? Datainicio { get; set; }

    public DateOnly? Datafim { get; set; }

    public decimal? Preco { get; set; }

    public string? Status { get; set; }

    public int? Projetoid { get; set; }

    public int? Utilizadorid { get; set; }

    public int? Relatorioprojid { get; set; }

    public virtual Projeto1? Projeto { get; set; }

    public virtual Relatorioproj? Relatorioproj { get; set; }

    public virtual Utilizador? Utilizador { get; set; }
}
