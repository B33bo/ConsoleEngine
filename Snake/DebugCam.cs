using ConsoleEngine;

namespace Snake;

internal class DebugCam : Camera
{
    public override void KeyPressed(ConsoleKey key)
    {
        if (key == ConsoleKey.I)
            Position += new Vector(0, -1);
        if (key == ConsoleKey.J)
            Position += new Vector(-1, 0);
        if (key == ConsoleKey.K)
            Position += new Vector(0, 1);
        if (key == ConsoleKey.L)
            Position += new Vector(1, 0);
    }
}
