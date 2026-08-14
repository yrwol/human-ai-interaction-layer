using Hail;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: hail <profile.yaml> [--output <path>]");
    return 1;
}

var profile = ProfileLoader.Load(args[0]);
var instructions = ClaudeCodeAdapter.Compile(profile);

if (args.Length >= 3 && args[1] == "--output")
{
    var outputPath = args[2];
    var directory = Path.GetDirectoryName(outputPath);

    if (!string.IsNullOrEmpty(directory))
    {
        Directory.CreateDirectory(directory);
    }

    File.WriteAllText(outputPath, instructions + Environment.NewLine);
    Console.WriteLine($"Wrote Claude Code instructions to {outputPath}");
    return 0;
}

Console.WriteLine(instructions);
return 0;
