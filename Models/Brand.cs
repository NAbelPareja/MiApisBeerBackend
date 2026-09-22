using System;
using System.Collections.Generic;

namespace MiApisBeer.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Beeer> Beeers { get; set; } = new List<Beeer>();
}
