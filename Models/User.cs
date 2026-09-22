using System;
using System.Collections.Generic;

namespace MiApisBeer.Models;

public partial class User
{
    public int UsedId { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}
