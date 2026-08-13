using Hail;

var profile = ProfileLoader.Load(args[0]);
Console.WriteLine($"HAIL profile v{profile.Version:0.0} loaded.");
