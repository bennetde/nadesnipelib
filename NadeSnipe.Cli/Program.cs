// namespace NadeSnipe.Cli;

using NadeSnipe;
using NadeSnipe.Annotations;
using NadeSnipe.Serializer;

var path = args.SingleOrDefault() ?? throw new Exception("Expected a single argument: <path to .dem>");

var lineups = new DemoLineupParser(File.OpenRead(path));
await lineups.Parse();

var fileStream = File.Create($"{path}.txt");
var serializer = new Kv3Serializer(fileStream);

serializer.Serialize(AnnotationFile.FromLineups(lineups.Lineups, lineups.MapName));
serializer.Flush();
Console.WriteLine($"Saved to {path}.txt");