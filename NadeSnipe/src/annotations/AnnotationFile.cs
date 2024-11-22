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

    public static AnnotationFile FromLineups(DemoLineupParser lineups) {
        AnnotationFile annotationFile = new();
        annotationFile.MapName = lineups.MapName;
        annotationFile.MapAnnotationNodes = lineups.Lineups.SelectMany(x => x.ToGrenadeNode()).Take(99).ToList();

        return annotationFile;
    }
}