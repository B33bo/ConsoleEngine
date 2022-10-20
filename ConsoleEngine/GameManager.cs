﻿using System.Diagnostics;

namespace ConsoleEngine;

public static class GameManager
{
    public static bool IsPlaying { get; private set; } = true;

    internal static List<GameObject> gameObjects = new();
    public static GameObject[] GameObjects => gameObjects.ToArray();

    private static readonly Stopwatch stopwatch = new();

    public static double Time => stopwatch.Elapsed.TotalSeconds;

    private static double timeOfLastRender;

    private static bool _doubleUp;
    public static bool DoubleUpCharacters
    {
        get => _doubleUp;
        set
        {
            _doubleUp = value;
            ScreenRefresh();
        }
    }

    internal static ConsoleKey? KeyPressed { get; set; }

    public static double MaxFPS
    {
        get => 1 / MinFrameDelta;
        set => MinFrameDelta = 1 / value;
    }

    public static double MinFrameDelta { get; set; }

    public static void Start()
    {
        stopwatch.Start();

        Task.Factory.StartNew(Input.GetKeyForever);
        ConsoleColorManager.Enable();
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        Console.Clear();
        Console.ResetColor();
        UpdateLoop();
    }

    #region AskQuestion
    public static string AskQuestion()
    {
        Input.AskQuestion(new VectorInt(0, GameWindow.ScreenDimensions.y + 2), null);
        while (Input.askingQuestion) { }
        return Input.GetAnswer;
    }

    public static string AskQuestion(VectorInt position)
    {
        Input.AskQuestion(position, null);
        while (Input.askingQuestion) { }
        return Input.GetAnswer;
    }

    public static string AskQuestion(int maxLength)
    {
        Input.AskQuestion(new VectorInt(0, GameWindow.ScreenDimensions.y + 2), maxLength);
        while (Input.askingQuestion) { }
        return Input.GetAnswer;
    }

    public static string AskQuestion(VectorInt position, int maxLength)
    {
        Input.AskQuestion(position, maxLength);
        while (Input.askingQuestion) { }
        return Input.GetAnswer;
    }
    #endregion

    public static void Stop()
    {
        IsPlaying = false;
        Renderer.DoRendering = false;
        Console.Clear();
        gameObjects.Clear();
        Console.WriteLine("Press any key to end");
    }

    private static void UpdateLoop()
    {
        while (IsPlaying)
        {
            Console.CursorVisible = false;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (!gameObjects[i].IsAlive)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                    continue;
                }

                gameObjects[i].Update(Time - timeOfLastRender);
            }

            //order matters

            if (KeyPressed.HasValue)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                    gameObjects[i].KeyPressed(KeyPressed.Value);
                KeyPressed = null;
            }

            Renderer.Render();

            timeOfLastRender = Time;
            while (Time - timeOfLastRender < MinFrameDelta) ;
        }
    }

    public static void AddObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
    }

    public static void ScreenRefresh()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
    }

    public static void LoadFromLevel(string[] level, Dictionary<char, Type> typesFromChar)
    {
        for (int y = 0; y < level.Length; y++)
        {
            for (int x = 0; x < level[y].Length; x++)
            {
                if (!typesFromChar.ContainsKey(level[y][x]))
                    continue;

                var type = typesFromChar[level[y][x]];

                if (Activator.CreateInstance(type) is not GameObject instance)
                    continue;
                instance.Position = new(x, y);
                GameManager.AddObject(instance);
            }
        }
    }
}