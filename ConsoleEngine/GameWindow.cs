namespace ConsoleEngine;

public sealed class GameWindow : GameObject
{
    private static GameWindow? _instance;
    public static GameWindow Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new GameWindow();
                GameManager.AddObject(_instance);
            }
            return _instance;
        }
    }

    private static VectorInt _screenDimensions;
    public static VectorInt ScreenDimensions
    {
        get => _screenDimensions; set
        {
            _screenDimensions = value;
            GameManager.ScreenRefresh();
        }
    }

    internal static void ResetInst()
    {
        _instance = null;
    }

    public GameWindow()
    {
        Invisible = true;
        ForegroundColor = Color.None;
        BackgroundColor = Color.None;
    }

    public static string HorizontalBar { get; set; } = "─";
    public static string VertictalBar { get; set; } = "│";
    public static string CornerBR { get; set; } = "┘";
    public static string CornerTR { get; set; } = "┐";
    public static string CornerBL { get; set; } = "└";
    public static string CornerTL { get; set; } = "┌";

    private Vector lastPosition;
    private Color lastBackgroundColor;
    private Color lastForegroundColor;

    public sealed override void Update(double deltaTime)
    {
        if (lastPosition != Position)
        {
            Reload();
            return;
        }

        if (lastBackgroundColor != BackgroundColor)
        {
            Reload();
            return;
        }

        if (lastForegroundColor != ForegroundColor)
        {
            Reload();
            return;
        }
    }

    private void Reload()
    {
        lastPosition = Position;
        lastBackgroundColor = BackgroundColor;
        lastForegroundColor = ForegroundColor;

        GameManager.ScreenRefresh();
    }

    public sealed override void OnDestroy()
    {
        _instance = null;
    }

    public sealed override void KeyPressed(ConsoleKey key)
    {
        if (key == ConsoleKey.F5)
            GameManager.ScreenRefresh();
        if (key == ConsoleKey.F6)
            DevConsole.ReadCommand();
    }
}
