using BenchmarkDotNet.Attributes;
using NadeSnipe;

public class BenchmarkLargeDemo {
    
    private string _path;

    // Read all contents to a buffer beforehand to remove System IO overehead from benchmarking.
    byte[] _buffer;
    public BenchmarkLargeDemo() {
        _path = Environment.CurrentDirectory + "/../../../../../../../../NadeSnipe.Tests/demos/example_ancient.dem";
        _buffer = File.ReadAllBytes(_path);
    }

    [Benchmark]
    public async Task Parse() => await new DemoLineupParser(new MemoryStream(_buffer)).Parse();
}