using System;
using System.Collections.Generic;

namespace AutoGenerateAPI.Database.Models;

public partial class Application
{
    public int AppId { get; set; }

    public int? HeroId { get; set; }

    public string? UserTableName { get; set; }

    public int? UserId { get; set; }

    public int? AddtionalInfoId { get; set; }

    public string? AdditionalInfoTableName { get; set; }
}
