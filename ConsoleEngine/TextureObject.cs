using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine;

public class TextureObject : GameObject
{
    public string[] Texture = Array.Empty<string>();

    public Color[,] Foreground = new Color[0, 0];
    public Color[,] Background = new Color[0, 0];

    public string TextureString
    {
        get
        {
            string s = "";
            for (int i = 0; i < Texture.Length; i++)
                s += "\n" + Texture[i];
            return s;
        }
        set
        {
            Texture = value.Trim('\r').Split('\n');
        }
    }

    public Color GetForegroundAt(int x, int y)
    {
        if (Foreground.GetLength(0) <= x)
            return ForegroundColor;
        if (Foreground.GetLength(1) <= y)
            return ForegroundColor;
        return Foreground[x, y];
    }

    public Color GetBackgroundAt(int x, int y)
    {
        if (Background.GetLength(0) <= x)
            return BackgroundColor;
        if (Background.GetLength(1) <= y)
            return BackgroundColor;
        return Background[x, y];
    }
}
