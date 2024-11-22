namespace NadeSnipe.Annotations;
using System.Numerics;

public class AimTargetGrenadeAnnotation : MapAnnotationNode
{
    public Guid MasterNodeId { get; set; }
    public AimTargetGrenadeAnnotation(Vector3 position, Vector3 angle, string title, string description, Guid masterNodeId) : base(position, angle, title, description)
    {
        MasterNodeId = masterNodeId;
        AnnotationType = AnnotationType.Grenade;
        SubType = SubType.AimTarget;
        Title = AnnotationText.FromMainTitle("Aim here");
        Desc = AnnotationText.FromMainDescription(description);
    }
}