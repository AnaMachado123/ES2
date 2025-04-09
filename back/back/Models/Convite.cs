using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Convite
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public DateOnly? Dataenvio { get; set; }

    public DateOnly? Dataresposta { get; set; }

    public int? Utilizadorid { get; set; }

    public int? Projetoid { get; set; }

    public virtual Projeto1? Projeto { get; set; }

    public virtual Utilizador? Utilizador { get; set; }
}
