using System;
using ConsoleEngine;

namespace Snake;

internal class TestObject : GameObject
{
    int c = 0;
    public TestObject()
    {
        Character = 'G';
        RenderOffscreen = true;
    }
    public override void Update(double deltaTime)
    {
        
    }

    public override void KeyPressed(ConsoleKey key)
    {
        Vector movement = new(0, 0);
        switch (key)
        {
            default:
                break;
            case ConsoleKey.W:
                movement = new(0, -1);
                break;
            case ConsoleKey.A:
                movement = new(-1, 0);
                break;
            case ConsoleKey.S:
                movement = new(0, 1);
                break;
            case ConsoleKey.D:
                movement = new(1, 0);
                break;
        }

        GameWindow.Instance.Position += movement;
    }
}
