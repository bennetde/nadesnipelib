namespace NadeSnipe.Annotations;
using System.Numerics;

public class MainGrenadeAnnotation : MapAnnotationNode
{
    public int StreakLimitGuidesOff { get; set; }
    public int StreakLimitGuidesOn { get; set; }
    public bool JumpThrow { get; set; }
    public MainGrenadeAnnotation(Vector3 position, Vector3 angle, string title, string description) : base(position, angle, title, description)
    {
        AnnotationType = AnnotationType.Grenade;
        SubType = SubType.Main;
        StreakLimitGuidesOff = 2;
        StreakLimitGuidesOn = 2;
        JumpThrow = false;
        Title = AnnotationText.FromMainTitle($"Thrown by {title}");
        Desc = AnnotationText.FromMainDescription(description);

    }
}