using NadeSnipe.Serializer.Attributes;

namespace NadeSnipe.Annotations;

public class AnnotationFile {
    public string MapName { get; set; }
    public ScreenText ScreenText { get; set; }

    [Kv3Unlist]
    public List<MapAnnotationNode> MapAnnotationNodes { get; set; }

    private AnnotationFile() {
        MapName = "";
        ScreenText = new();
        MapAnnotationNodes = new();
    }

    public static AnnotationFile FromLineups(List<Lineup> lineups, string mapName) {
        AnnotationFile annotationFile = new();
        annotationFile.MapName = mapName;
        annotationFile.MapAnnotationNodes = lineups.SelectMany(x => x.ToGrenadeNode()).Take(99).ToList();

        return annotationFile;
    }
}