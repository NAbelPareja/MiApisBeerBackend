using System;
using System.Collections.Generic;

namespace MiApisBeer.Models;

public partial class Proveedore
{
    public int ProveedoresId { get; set; }

    public string Name { get; set; } = null!;

    public string Ruc { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
}
