namespace NadeSnipe.Math;
using System.Numerics;

public static class RotateExtension {

    /// <summary>
    /// Rotate a vector
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="x">Pitch in radians</param>
    /// <param name="y">Yaw in radians</param>
    /// <returns>The rotated Vector</returns>
    public static Vector3 Rotate(this Vector3 vector, float x, float y) {
        // // Roll
        // var Rx = new Matrix4x4(1, 0,            0,             0,
        //                        0, MathF.Cos(z), -MathF.Sin(z), 0,
        //                        0, MathF.Sin(z), MathF.Cos(z),  0,
        //                        0, 0,             0,            1);
        // Pitch
        var Ry = new Matrix4x4(MathF.Cos(x),  0, MathF.Sin(x), 0,
                               0,             1, 0,            0,
                               -MathF.Sin(x), 0, MathF.Cos(x), 0,
                               0,             0, 0,            1);

        // Yaw
        var Rz = new Matrix4x4(MathF.Cos(y), -MathF.Sin(y), 0, 0,
                               MathF.Sin(y), MathF.Cos(y),  0, 0,
                               0,            0,             1, 0,
                               0,            0,             0, 1);

        // var rotated = Vector3.Transform(vector, Ry);
        var r = Matrix4x4.Multiply(Ry, Rz);
        var rotated = System.Numerics.Vector3.Transform(vector, r);

        return rotated;
    }
}