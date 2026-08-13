using Hail;

var profile = ProfileLoader.Load(args[0]);
var instructions = ClaudeCodeAdapter.Compile(profile);

Console.WriteLine(instructions);
