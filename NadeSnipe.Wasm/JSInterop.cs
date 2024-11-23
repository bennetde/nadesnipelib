using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using NadeSnipe.Annotations;
using NadeSnipe.Math;
using NadeSnipe.Serializer;

namespace NadeSnipe.Wasm;

public static partial class JSInterop
{

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Vector3))]
    [JsonSerializable(typeof(List<Lineup>))]
    public partial class SourceGenerationContext : JsonSerializerContext
    {
    }

    [JSExport]
    internal static async Task<string> ReadDemo(byte[] buffer) {
        Stream stream = new MemoryStream(buffer);
        var demo = new DemoLineupParser(stream);
        await demo.Parse();

        return JsonSerializer.Serialize(demo);
    }

    [JSExport]
    public static string SerializeToKv3(string lineupsJson, string mapName) {
        // List<Lineup> lineups = JsonSerializer.Deserialize<List<Lineup>>(lineupsJson)!;
        var lineups = JsonSerializer.Deserialize(lineupsJson, typeof(List<Lineup>), SourceGenerationContext.Default) as List<Lineup>;
        var annotationFile = AnnotationFile.FromLineups(lineups, mapName);
        var ms = new MemoryStream();
        Kv3Serializer serializer = new Kv3Serializer(ms);
        serializer.Serialize(annotationFile);
        serializer.Flush();
        ms.Position = 0;
        var streamReader = new StreamReader(ms, Encoding.UTF8);
        string output = streamReader.ReadToEnd();
        return output;
        // return String.Join(", ", lineups!.Select(x => x.PlayerName).ToArray());
    }
} 