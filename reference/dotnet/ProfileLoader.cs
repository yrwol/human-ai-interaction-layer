using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hail;

public static class ProfileLoader
{
    public static HailProfile Load(string path)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var yaml = File.ReadAllText(path);
        return deserializer.Deserialize<HailProfile>(yaml);
    }
}
