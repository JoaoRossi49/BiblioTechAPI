using System;
using System.Collections.Generic;

namespace BiblioTechAPI.Models;

public partial class Livro
{
    public int IdLivo { get; set; }

    public string NomeLivro { get; set; } = null!;

    public int IdGenero { get; set; }

    public int QuantidadePaginas { get; set; }

    public int IdAutor { get; set; }
}
