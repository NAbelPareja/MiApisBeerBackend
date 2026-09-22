using System;
using System.Collections.Generic;

namespace MiApisBeer.Models;

public partial class Beeer
{
    public int BeerId { get; set; }

    public string? Name { get; set; }

    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }
}
