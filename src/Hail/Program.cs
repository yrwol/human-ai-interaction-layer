using Hail;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: hail <profile.yaml> [--output <path> | --install claude-code [--home <path>]]");
    return 1;
}

var profile = ProfileLoader.Load(args[0]);
var instructions = ClaudeCodeAdapter.Compile(profile);

if (args.Length >= 3 && args[1] == "--output")
{
    var outputPath = args[2];
    var directory = Path.GetDirectoryName(outputPath);
    if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
    File.WriteAllText(outputPath, instructions + Environment.NewLine);
    Console.WriteLine($"Wrote Claude Code instructions to {outputPath}");
    return 0;
}

if (args.Length >= 3 && args[1] == "--install" && args[2] == "claude-code")
{
    string? home = null;
    if (args.Length >= 5 && args[3] == "--home") home = args[4];
    ClaudeCodeInstaller.Install(instructions, home);
    Console.WriteLine("Installed HAIL interaction instructions for Claude Code.");
    return 0;
}

Console.WriteLine(instructions);
return 0;
