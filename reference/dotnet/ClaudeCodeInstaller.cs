namespace Hail;

public static class ClaudeCodeInstaller
{
    public const string ImportLine = "@~/.hail/claude-code.md";

    public static void Install(string instructions, string? homeDirectory = null)
    {
        var home = homeDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var hailDirectory = Path.Combine(home, ".hail");
        var claudeDirectory = Path.Combine(home, ".claude");
        var hailInstructionsPath = Path.Combine(hailDirectory, "claude-code.md");
        var claudeMemoryPath = Path.Combine(claudeDirectory, "CLAUDE.md");

        Directory.CreateDirectory(hailDirectory);
        Directory.CreateDirectory(claudeDirectory);
        File.WriteAllText(hailInstructionsPath, instructions + Environment.NewLine);

        var existing = File.Exists(claudeMemoryPath) ? File.ReadAllText(claudeMemoryPath) : string.Empty;
        if (existing.Split('\n').Any(line => line.TrimEnd('\r').Trim() == ImportLine)) return;

        using var writer = File.AppendText(claudeMemoryPath);
        if (existing.Length > 0 && !existing.EndsWith('\n')) writer.WriteLine();
        writer.WriteLine(ImportLine);
    }
}
