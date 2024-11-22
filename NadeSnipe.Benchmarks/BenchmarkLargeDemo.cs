using BenchmarkDotNet.Attributes;
using NadeSnipe;

public class BenchmarkLargeDemo {
    
    private string _path;
    // private FileStream _stream;
    public BenchmarkLargeDemo() {
        _path = Environment.CurrentDirectory + "/../../../../../../../../NadeSnipe.Tests/demos/example_ancient.dem";
    }

    [Benchmark]
    public async Task Parse() => await new DemoLineupParser(File.OpenRead(_path)).Parse();
}