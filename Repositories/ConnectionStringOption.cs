using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories;

public sealed record ConnectionStringOption
{
    // should have the same name as in appsettings 
    public const string Key = "ConnectionStrings";

    public string SqlServer { get; set; } = default!;
}

