namespace NadeSnipe.Annotations;
using System.Numerics;
using System.Text.Unicode;
using DemoFile;
using NadeSnipe.Serializer.Attributes;

public enum AnnotationType {
    [Kv3StringValue("grenade")]
    Grenade,
    [Kv3StringValue("position")]
    Position
    //TODO: Add more Types here
}

public enum SubType {
    [Kv3StringValue("main")]
    Main,
    [Kv3StringValue("aim_target")]
    AimTarget,
    [Kv3StringValue("destination")]
    Destination
}

public enum AnnotationTextAlign {
    [Kv3StringValue("left")]
    Left,
    [Kv3StringValue("center")]
    Center,
    [Kv3StringValue("right")]
    Right
}


public abstract class MapAnnotationNode {

    const float VERT_PLAYER_OFFSET = 60.0f;

    public bool Enabled { get;set; }

    [Kv3Key("Type")]
    public AnnotationType AnnotationType { get; set; }
    [Kv3ToString]
    public Guid Id { get; set; }
    public SubType SubType { get; set; }
    public float[] Position { get; set; }
    public float[] Angles { get; set; }
    public bool VisiblePfx { get; set; }
    public byte[]? Color { get; set; }
    public float[] TextPositionOffset { get; set; }
    public bool TextFacePlayer { get; set; }
    public AnnotationTextAlign TextHorizontalAlign { get; set; }
    public bool RevealOnSuccess { get; set; }
    public AnnotationText Title { get; set; }
    public AnnotationText Desc { get; set; }
    // public int? StreakLimitGuidesOn { get; set; }
    // public int? StreakLimitGuidesOff { get; set; }
    // public bool? JumpThrow { get; set; }
    // [Kv3ToString]
    // public Guid MasterNodeId { get; set; }

    public MapAnnotationNode(Vector3 position, Vector3 angle, string title, string description) {
        Enabled = true;
        Id = Guid.NewGuid();
        Position = new float[3];
        Position[0] = position.X;
        Position[1] = position.Y;
        Position[2] = position.Z;
        Angles = new float[3];
        Angles[0] = angle.X;
        Angles[1] = angle.Y;
        Angles[2] = angle.Z;
        VisiblePfx = true;
        Color = [255,255,255];
        TextPositionOffset = [0.0f, 0.0f, 60.0f];
        TextFacePlayer = true;
        TextHorizontalAlign = AnnotationTextAlign.Center;
        RevealOnSuccess = false;
        Title = new AnnotationText();
        Desc = new AnnotationText();
        // StreakLimitGuidesOn = 2;
        // StreakLimitGuidesOff = 2;
        // JumpThrow = false;
    }
}