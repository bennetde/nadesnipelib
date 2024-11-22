// namespace NadeSnipe.Cli;

using NadeSnipe;
using NadeSnipe.Annotations;
using NadeSnipe.Serializer;

var path = args.SingleOrDefault() ?? throw new Exception("Expected a single argument: <path to .dem>");
// var path = args[0];

var lineups = new DemoLineupParser(File.OpenRead(path));
await lineups.Parse();
// Console.WriteLine(lineups);

var fileStream = File.Create($"{path}.txt");
var serializer = new Kv3Serializer(fileStream);

serializer.Serialize(AnnotationFile.FromLineups(lineups));
serializer.Flush();
Console.WriteLine($"Saved to {path}.txt");

// Console.WriteLine(JsonSerializer.Serialize(Vector3.One));