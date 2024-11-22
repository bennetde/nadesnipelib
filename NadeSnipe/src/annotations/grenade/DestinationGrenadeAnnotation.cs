namespace NadeSnipe.Annotations;
using System.Numerics;

public class DestinationGrenadeAnnotation : MapAnnotationNode
{
    public Guid MasterNodeId { get; set; }
    public float DistanceThreshold { get; set; }
    public DestinationGrenadeAnnotation(Vector3 position, Vector3 angle, string title, string description, Guid masterNodeId, float distanceThreshold) : base(position, angle, title, description)
    {
        MasterNodeId = masterNodeId;
        DistanceThreshold = distanceThreshold;
        SubType = SubType.Destination;
    }
}