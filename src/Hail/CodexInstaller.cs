namespace Hail;

public static class CodexInstaller
{
    public const string StartMarker = "<!-- HAIL:START -->";
    public const string EndMarker = "<!-- HAIL:END -->";

    public static void Install(string instructions, string? homeDirectory = null)
    {
        var home = homeDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var codexHome = Environment.GetEnvironmentVariable("CODEX_HOME");
        if (string.IsNullOrWhiteSpace(codexHome))
            codexHome = Path.Combine(home, ".codex");

        Directory.CreateDirectory(codexHome);
        var agentsPath = Path.Combine(codexHome, "AGENTS.md");
        var existing = File.Exists(agentsPath) ? File.ReadAllText(agentsPath) : string.Empty;

        var managedBlock = $"{StartMarker}{Environment.NewLine}{instructions}{Environment.NewLine}{EndMarker}";
        var start = existing.IndexOf(StartMarker, StringComparison.Ordinal);
        var end = existing.IndexOf(EndMarker, StringComparison.Ordinal);

        string updated;
        if (start >= 0 && end > start)
        {
            end += EndMarker.Length;
            updated = existing[..start] + managedBlock + existing[end..];
        }
        else
        {
            var separator = existing.Length == 0 ? string.Empty : existing.EndsWith('\n') ? Environment.NewLine : Environment.NewLine + Environment.NewLine;
            updated = existing + separator + managedBlock + Environment.NewLine;
        }

        File.WriteAllText(agentsPath, updated);
    }
}
