namespace NadeSnipe.Annotations;

public class AnnotationText {
    public string Text { get; set; }
    public uint FontSize { get; set; }
    public float FadeInDist { get; set; }
    public float FadeOutDist { get; set; }

    public AnnotationText(string text, uint fontSize, float fadeInDist, float fadeOutDist) {
        Text = text;
        FontSize = fontSize;
        FadeInDist = fadeInDist;
        FadeOutDist = fadeOutDist;
    }

    public AnnotationText() {
        Text = "";
        FontSize = 125;
        FadeInDist = 600.0f;
        FadeOutDist = 40.0f;
    }


    public static AnnotationText FromMainTitle(string text) {
        return new AnnotationText(text, 125, 600.0f, 40.0f);
    }

    public static AnnotationText FromMainDescription(string text) {
        return new AnnotationText(text, 75, 300.0f, 40.0f);
    }

    public static AnnotationText FromAimTargetDescription(string text) {
        return new AnnotationText(text, 75, 50.0f, -1.0f);
    }

    public static AnnotationText FromAimTargetTitle(string text) {
        return new AnnotationText(text, 125, 50.0f, -1.0f);
    }
}
