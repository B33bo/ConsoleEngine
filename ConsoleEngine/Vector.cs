namespace ConsoleEngine;

public struct Vector
{
    public double x, y;

    public Vector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsInBoundsScreenSpace
    {
        get =>
            x >= 0 && x < GameWindow.ScreenDimensions.x &&
            y >= 0 && y < GameWindow.ScreenDimensions.y;
    }

    public bool IsInBounds
    {
        get
        {
            double tru_x = ToScreenSpace.x;
            double tru_y = ToScreenSpace.y;
            return tru_x >= 0 && tru_x < GameWindow.ScreenDimensions.x &&
            tru_y >= 0 && tru_y < GameWindow.ScreenDimensions.y;
        }
    }

    public Vector ToScreenSpace => Camera.MainCamera is null ? this : this - Camera.MainCamera.Position;
    public Vector ToGameSpace => Camera.MainCamera is null ? this : this + Camera.MainCamera.Position;

    public static Vector operator +(Vector a, Vector b) =>
        new(a.x + b.x, a.y + b.y);

    public static Vector operator -(Vector a, Vector b) =>
        new(a.x - b.x, a.y - b.y);

    public static Vector operator -(Vector a) =>
        new(-a.x, -a.y);

    public static Vector operator *(Vector a, double b) =>
        new(a.x * b, a.y * b);

    public static Vector operator /(Vector a, double b) =>
        new(a.x / b, a.y / b);

    public static bool operator ==(Vector a, Vector b) =>
        a.x == b.x && a.y == b.y;

    public static bool operator !=(Vector a, Vector b) =>
        !(a.x == b.x && a.y == b.y);

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        return this == (Vector)obj;
    }

    public override int GetHashCode() =>
        base.GetHashCode();

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}

public struct VectorInt
{
    public static readonly VectorInt Zero = new(0, 0);
    public int x, y;

    public bool IsInBoundsScreenSpace
    {
        get =>
            x >= 0 && x < GameWindow.ScreenDimensions.x &&
            y >= 0 && y < GameWindow.ScreenDimensions.y;
    }

    public bool IsInBounds
    {
        get
        {
            int tru_x = ToScreenSpace.x;
            int tru_y = ToScreenSpace.y;
            return tru_x >= 0 && tru_x < GameWindow.ScreenDimensions.x &&
            tru_y >= 0 && tru_y < GameWindow.ScreenDimensions.y;
        }
    }

    public VectorInt ToScreenSpace => Camera.MainCamera is null ? this : this - Camera.MainCamera.PositionInt;
    public VectorInt ToGameSpace => Camera.MainCamera is null ? this : this + Camera.MainCamera.PositionInt;

    public VectorInt(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector(VectorInt v)
    {
        return new Vector(v.x, v.y);
    }

    public static explicit operator VectorInt(Vector v)
    {
        return new VectorInt((int)v.x, (int)v.y);
    }

    public static VectorInt operator +(VectorInt a, VectorInt b) =>
        new(a.x + b.x, a.y + b.y);

    public static VectorInt operator -(VectorInt a, VectorInt b) =>
        new(a.x - b.x, a.y - b.y);

    public static VectorInt operator -(VectorInt a) =>
        new(-a.x, -a.y);

    public static VectorInt operator *(VectorInt a, double b) =>
        new((int)(a.x * b), (int)(a.y * b));

    public static VectorInt operator /(VectorInt a, double b) =>
        new((int)(a.x / b), (int)(a.y / b));

    public static bool operator ==(VectorInt a, VectorInt b) =>
        a.x == b.x && a.y == b.y;

    public static bool operator !=(VectorInt a, VectorInt b) =>
        !(a.x == b.x && a.y == b.y);

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        return this == (VectorInt)obj;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public override int GetHashCode() =>
        base.GetHashCode();
}