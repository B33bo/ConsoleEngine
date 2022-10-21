using ConsoleEngine;
using ConsoleEngine.Sound;

namespace Snake;

public static class Program
{
    public static void Main(string[] args)
    {
        GameWindow.ScreenDimensions = new VectorInt(15, 15);
        GameManager.MaxFPS = 60;

        Snake snake = new();

        FPSreader fpsreader = new()
        {
            Position = new(0, GameWindow.ScreenDimensions.y + 3),
            RenderOffscreen = true,
        };

        Console.Title = "Snake";
        GameManager.DoubleUpCharacters = true;
        GameWindow.Instance.ForegroundColor = new Color(255, 255, 0);
        GameWindow.Instance.Position = new(0, 0);
        GameManager.AddObject(snake);
        GameManager.AddObject(fpsreader);

        GameManager.Start();
    }
}