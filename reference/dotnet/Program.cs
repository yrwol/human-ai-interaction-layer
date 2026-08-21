using Hail;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: hail <profile.yaml> [--target claude-code|codex] [--output <path> | --install claude-code|codex [--home <path>]]");
    return 1;
}

var profile = ProfileLoader.Load(args[0]);
var target = "claude-code";

for (var i = 1; i < args.Length - 1; i++)
{
    if (args[i] == "--target") target = args[i + 1];
}

string Compile() => target switch
{
    "codex" => CodexAdapter.Compile(profile),
    _ => ClaudeCodeAdapter.Compile(profile)
};

var instructions = Compile();

for (var i = 1; i < args.Length; i++)
{
    if (args[i] == "--output" && i + 1 < args.Length)
    {
        var outputPath = args[i + 1];
        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        File.WriteAllText(outputPath, instructions + Environment.NewLine);
        Console.WriteLine($"Wrote {target} instructions to {outputPath}");
        return 0;
    }

    if (args[i] == "--install" && i + 1 < args.Length)
    {
        var installTarget = args[i + 1];
        string? home = null;
        for (var j = i + 2; j < args.Length - 1; j++)
        {
            if (args[j] == "--home") home = args[j + 1];
        }

        if (installTarget == "codex")
        {
            CodexInstaller.Install(CodexAdapter.Compile(profile), home);
            Console.WriteLine("Installed HAIL interaction instructions for Codex.");
            return 0;
        }

        ClaudeCodeInstaller.Install(ClaudeCodeAdapter.Compile(profile), home);
        Console.WriteLine("Installed HAIL interaction instructions for Claude Code.");
        return 0;
    }
}

Console.WriteLine(instructions);
return 0;
