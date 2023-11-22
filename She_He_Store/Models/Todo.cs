using System;
using System.Collections.Generic;

namespace She_He_Store.Models;

public partial class Todo
{
    public decimal Todolistid { get; set; }

    public string Tododescription { get; set; } = null!;

    public string? Todostatues { get; set; }
}
