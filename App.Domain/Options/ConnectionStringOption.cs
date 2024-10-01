namespace App.Domain.Options;

public sealed record ConnectionStringOption
{
    // should have the same name as in appsettings 
    public const string Key = "ConnectionStrings";

    public string SqlServer { get; set; } = default!;
}

