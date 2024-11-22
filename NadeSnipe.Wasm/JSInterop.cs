using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace NadeSnipe.Wasm;

public static partial class JSInterop
{
    [JSExport]
    internal static async Task<string> ReadDemo(byte[] buffer) {
        Stream stream = new MemoryStream(buffer);
        var demo = new DemoLineupParser(stream);
        await demo.Parse();

        return JsonSerializer.Serialize(demo);
    }
} 