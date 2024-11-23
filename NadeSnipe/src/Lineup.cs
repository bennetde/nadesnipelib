using System.Text.Json.Serialization;
using NadeSnipe.Annotations;
using NadeSnipe.Math;

namespace NadeSnipe;

public enum GrenadeType {
    HE, Flashbang, Molotov, Smoke, Decoy
}

public enum ThrowType {
    Normal = 0b0, 
    Forward = 0b1, 
    Jump = 0b10, 
    ForwardJump = 0b11, 
    Crouch = 0b100, 
    CrouchJump = 0b110, 
    ForwardCrouchJump = 0b111,
    ForwardCrouch = 0b101,
}
public enum Team {
    CounterTerrorist,
    Terrorist
}

public class Lineup {
    public Vector3 Origin { get; set; }
    public Vector3 EyeOffset { get; set; }
    public Vector3 Angle { get; set; }
    public string PlayerName { get; set; }
    public GrenadeType GrenadeType { get; set; }
    public ThrowType ThrowType { get; set; }
    public Vector3? DetonationOrigin { get; set; }
    public Team Team { get; set;}

    public bool IsJumpThrow() {
        return ((int)ThrowType & 0b10) > 0;
    }

    public Lineup(Vector3 origin, Vector3 angle, Vector3 eyePos, string playerName, GrenadeType grenadeType, ThrowType throwType, Team team) {
        Origin = origin;
        Angle = angle;
        EyeOffset = eyePos;
        PlayerName = playerName;
        GrenadeType = grenadeType;
        ThrowType = throwType;
        Team = team;
    }

    /// <summary>
    /// Needed for JSON deserialization. Do not use.
    /// </summary>
    [JsonConstructor]
    public Lineup() {
        // Origin = Vector3.Zero;
        // Angle = Vector3.Zero;
        // EyeOffset = Vector3.Zero;
        // PlayerName = "";
        // GrenadeType = GrenadeType.HE;
        // ThrowType = ThrowType.Normal;
        // Team = Team.Terrorist;
    }


    public MapAnnotationNode ToPositionNode() {
        var node = new PositionAnnotationNode(Origin, Angle, PlayerName, "");
        return node;
    }

    public MapAnnotationNode[] ToGrenadeNode() {
        const float DegToRad = (float)System.Math.PI / 180.0f;
        float x = DegToRad * -Angle.X; // Pitch
        float y = DegToRad * -Angle.Y; // Yaw

        var forward = Vector3.UnitX * 100.0f;
        var rotated = forward.Rotate(x,y);
        
        var aimTargetOrigin = Origin + rotated + Vector3.UnitZ * EyeOffset.Z;

        MapAnnotationNode[] nodes = DetonationOrigin != Vector3.Zero ? new MapAnnotationNode[3] : new MapAnnotationNode[2];
        var mainNode = new MainGrenadeAnnotation(Origin, new Vector3(0.0f, Angle.Y, 0.0f), PlayerName, "");
        mainNode.JumpThrow = IsJumpThrow();
        var aimTargetNode = new AimTargetGrenadeAnnotation(aimTargetOrigin, Angle, "Aim Target", ThrowType.ToString(), mainNode.Id);
        

        aimTargetNode.TextPositionOffset = [0.0f, 0.0f, 30.0f];

        nodes[0] = mainNode;
        nodes[1] = aimTargetNode;

        if(DetonationOrigin != null) {
            var destinationNode = new DestinationGrenadeAnnotation(DetonationOrigin, Angle, "", "", mainNode.Id, 80.0f);
            nodes[2] = destinationNode;
        }


        return nodes;
    }
}