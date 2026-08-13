namespace Hail;

public sealed record HailProfile(
    double Version,
    InteractionPreferences Profile);

public sealed record InteractionPreferences(
    string Verbosity,
    string DecisionMode,
    int MaxOptions,
    string TaskChunking,
    string TangentPolicy);
