using System;
using System.Collections.Generic;

namespace AutoGenerateAPI.Database.Models;

public partial class HeroTable
{
    public int HeroId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
