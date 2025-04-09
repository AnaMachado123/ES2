using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Relatorio
{
    public int Id { get; set; }

    public int Mes { get; set; }

    public int Ano { get; set; }

    public decimal? Totalhoras { get; set; }

    public decimal? Precototal { get; set; }

    public decimal? Precohora { get; set; }

    public int? Numprojeto { get; set; }

    public int? Utilizadorid { get; set; }

    public virtual Utilizador? Utilizador { get; set; }
}
