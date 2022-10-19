using System.Globalization;

namespace ConsoleEngine;

public struct Color
{
    public static bool ColorsAllowed { get; internal set; }
    public byte R;
    public byte G;
    public byte B;
    public bool Enabled = false;
    public static readonly Color None = new(false);

    public Color(byte r, byte g, byte b)
    {
        R = r; G = g; B = b;
        Enabled = true;
    }

    private Color(bool enabled)
    {
        R = 0; G = 0; B = 0;
        Enabled = enabled;
    }

    public override string ToString()
    {
        return R.ToString("x").PadLeft(2, '0') + G.ToString("x").PadLeft(2, '0') + B.ToString("x").PadLeft(2, '0');
    }

    public static Color operator -(Color color) =>
        new((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));

    public static bool operator ==(Color a, Color b) =>
        a.R == b.R && a.G == b.G && a.B == b.B;

    public static bool operator !=(Color a, Color b)
        => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is not Color col)
            return false;
        return this == col;
    }

    public override int GetHashCode() =>
        base.GetHashCode();

    public static Color Parse(string s)
    {
        if (TryParse(s, out Color c))
            return c;
        throw new FormatException("Input string was not in a correct format.");
    }

    public static bool TryParse(string s, out Color c)
    {
        if (s.StartsWith("#"))
            s = s[1..];

        c = Color.None;
        if (s.Length != 6)
            return false;

        if (s == "nocolr")
            return true;

        byte red = byte.Parse(s[..2], NumberStyles.HexNumber);
        byte green = byte.Parse(s[2..4], NumberStyles.HexNumber);
        byte blue = byte.Parse(s[4..6], NumberStyles.HexNumber);

        c = new Color(red, green, blue);
        return true;
    }
}

public struct ColorHSV
{
    public float Hue; //0-360
    public float Saturation; //0-1
    public float Value; //0-1
    public bool Enabled = false;

    public ColorHSV(float hue, float saturation, float value)
    {
        Hue = hue;
        Saturation = saturation;
        Value = value;
        Enabled = true;
    }
    public override string ToString()
    {
        return $"{Hue} {Saturation} {Value}";//((Color)this).ToString();
    }

    public static implicit operator Color(ColorHSV hsv)
    {
        if (hsv.Saturation == 0)
        {
            byte value = (byte)(hsv.Value * 255);
            return new Color(value, value, value);
        }

        hsv.Hue %= 360;

        float M = 255 * hsv.Value;
        float m = M * (1 - hsv.Saturation);

        float z = (M - m) * (1 - Math.Abs((hsv.Hue / 60 % 2) - 1));

        if (hsv.Hue < 60)
            return new Color((byte)M, (byte)(z + m), (byte)m);

        if (hsv.Hue < 120)
            return new Color((byte)(z + m), (byte)M, (byte)m);

        if (hsv.Hue < 180)
            return new Color((byte)m, (byte)M, (byte)(z + m));

        if (hsv.Hue < 240)
            return new Color((byte)m, (byte)(z + m), (byte)M);

        if (hsv.Hue < 300)
            return new Color((byte)(z + m), (byte)m, (byte)M);

        if (hsv.Hue < 360)
            return new Color((byte)M, (byte)m, (byte)(z + m));

        throw new Exception("Hue value not between 0 and 360");
    }

    public static implicit operator ColorHSV(Color rgb)
    {
        byte max = Max(rgb.R, rgb.B, rgb.G);
        byte min = Min(rgb.R, rgb.B, rgb.G);

        float value = max / 255f;
        float saturation = max > 0 ? (1 - (min / (float)max)) : 0;

        float R = rgb.R;
        float G = rgb.G;
        float B = rgb.B;

        const float rad2Deg = 180 / MathF.PI;

        float hue = MathF.Acos((R - (G / 2) - (B / 2)) / MathF.Sqrt(R * R + G * G + B * B - R * G - R * B - G * B)) * rad2Deg; //dafuck

        if (B > G)
            hue = 360 - hue;

        return new ColorHSV(hue, saturation, value);

        static byte Max(byte a, byte b, byte c)
        {
            if (a > b)
                return a > c ? a : c;
            return b > c ? b : c;
        }

        static byte Min(byte a, byte b, byte c)
        {
            if (a < b)
                return a < c ? a : c;
            return b < c ? b : c;
        }
    }

    public static ColorHSV operator -(ColorHSV color) =>
        -(Color)color;

    public static bool operator ==(ColorHSV a, ColorHSV b)
    {
        if (Math.Floor(a.Hue) != Math.Floor(b.Hue))
            return false;
        return a.Saturation == b.Saturation && a.Value == b.Value;
    }

    public static bool operator !=(ColorHSV a, ColorHSV b) 
        => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is not ColorHSV col)
            return false;
        return this == col;
    }

    public override int GetHashCode() =>
        base.GetHashCode();
}
