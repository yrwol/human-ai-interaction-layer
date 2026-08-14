namespace Hail;

public sealed class HailProfile
{
    public double Version { get; set; }
    public InteractionPreferences Profile { get; set; } = new();
}

public sealed class InteractionPreferences
{
    public string Verbosity { get; set; } = string.Empty;
    public string DecisionMode { get; set; } = string.Empty;
    public int MaxOptions { get; set; }
    public string TaskChunking { get; set; } = string.Empty;
    public string TangentPolicy { get; set; } = string.Empty;
}
