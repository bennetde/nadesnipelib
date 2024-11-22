namespace NadeSnipe.Annotations;
using System.Numerics;

public class PositionAnnotationNode : MapAnnotationNode
{
    public PositionAnnotationNode(Vector3 position, Vector3 angle, string title, string description) : base(position, angle, title, description)
    {
        AnnotationType = AnnotationType.Position;
        SubType = SubType.Main;
    }
}