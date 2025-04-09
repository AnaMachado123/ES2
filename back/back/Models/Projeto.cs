using System;
using System.Collections.Generic;

namespace back.Models;

public partial class Projeto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public decimal PrecoHora { get; set; }
}
