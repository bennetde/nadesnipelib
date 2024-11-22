using DemoFile;

namespace NadeSnipe.Math;

public class Vector3 {
    public float X {get; set;}
    public float Y {get; set;}
    public float Z {get; set;}

    public static Vector3 UnitX => new Vector3(1.0f, 0.0f, 0.0f);
    public static Vector3 UnitY => new Vector3(0.0f, 1.0f, 0.0f);
    public static Vector3 UnitZ => new Vector3(0.0f, 0.0f, 1.0f);
    public static Vector3 One => new Vector3(1.0f, 1.0f, 1.0f);
    public static Vector3 Zero => new Vector3(0.0f, 0.0f, 0.0f);

    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(Vector vector) {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
    }

    public float Magnitude() {
        return MathF.Sqrt(MathF.Pow(X, 2) + MathF.Pow(Y, 2) + MathF.Pow(Z, 2));
    }

    public float Distance(Vector3 vector) {
        return MathF.Sqrt(MathF.Pow(X - vector.X, 2) + MathF.Pow(Y - vector.Y, 2) + MathF.Pow(Z - vector.Z, 2));
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b) {
        return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3 operator *(Vector3 a, Vector3 b) {
        return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }

    public static Vector3 operator *(Vector3 a, float b) {
        return new Vector3(a.X * b, a.Y * b, a.Z * b);
    }

    public static Vector3 operator /(Vector3 a, float b) {
        return new Vector3(a.X / b, a.Y / b, a.Z / b);
    }

    public static implicit operator System.Numerics.Vector3(Vector3 a) => new System.Numerics.Vector3(a.X, a.Y, a.Z);
    public static implicit operator Vector3(System.Numerics.Vector3 a) => new Vector3(a.X, a.Y, a.Z);

    public override string ToString()
    {
        return $"({X} {Y} {Z})";
    }
}