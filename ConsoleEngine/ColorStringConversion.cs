using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine;

internal static class ColorStringConversion
{
    public const string START_COLOR = "\u001b";
    public const string END_COLOR = "\u001b[0m";

    public const int FOREGROUND = 38;
    public const int BACKGROUND = 48;

    public static string ToConsoleColor(string text, Color foreground, Color background)
    {
        if (!Color.ColorsAllowed)
            return text;

        string colorInformation = "";

        if (foreground.Enabled)
            colorInformation += $"{START_COLOR}[{FOREGROUND};2;{foreground.R};{foreground.G};{foreground.B}m";

        if (background.Enabled)
            colorInformation += $"{START_COLOR}[{BACKGROUND};2;{background.R};{background.G};{background.B}m";

        if (colorInformation == "")
            return text;

        colorInformation += text + END_COLOR;
        return colorInformation;
    }

    public static string BeginColor(this Color color, bool foreground)
    {
        if (!color.Enabled)
            return "";
        if (foreground)
            return $"{START_COLOR}[{FOREGROUND};2;{color.R};{color.G};{color.B}m";
        return $"{START_COLOR}[{BACKGROUND};2;{color.R};{color.G};{color.B}m";
    }
}
