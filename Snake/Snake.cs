using ConsoleEngine;
using ConsoleEngine.Sound;

namespace Snake;

internal class Snake : GameObject
{
    public static int Score { get; private set; }

    private Vector direction;
    private Vector directionOfLastMove;

    private float speed = 5;
    private readonly GameObject apple;
    private readonly List<GameObject> tails;
    private readonly Random random;

    private readonly TextObject scoreText;

    private List<PianoKey> keys = new();

    public Snake()
    {
        apple = new()
        {
            Position = new Vector(5, 5),
            Character = '█',
            ForegroundColor = new(255, 0, 0),
        };

        Character = '█';
        CollisionEnabled = true;
        RenderOffscreen = true;

        scoreText = new()
        {
            Text = "Score: 0",
            RenderOffscreen = true,
            Position = new(0, GameWindow.ScreenDimensions.y + 2),
        };

        random = new Random();
        tails = new List<GameObject>();
        speed = 1;
        GameManager.AddObject(apple);
        GameManager.AddObject(scoreText);
    }

    public override void Update(double deltaTime)
    {
        VectorInt lastPosInt = PositionInt;

        Position += direction * deltaTime * speed;

        if (lastPosInt != PositionInt)
        {
            //moved a whole block
            directionOfLastMove = direction;
            UpdateTails(lastPosInt);

            if (!PositionInt.IsInBoundsScreenSpace)
                DieManager.Die();
        }
    }

    public override void KeyPressed(ConsoleKey key)
    {
        Vector newDirection = key switch
        {
            ConsoleKey.W => new(0, -1),
            ConsoleKey.A => new(-1, 0),
            ConsoleKey.S => new(0, 1),
            ConsoleKey.D => new(1, 0),
            _ => direction,
        };

        if (key == ConsoleKey.Escape)
            GameManager.Stop();

        if (-newDirection == directionOfLastMove)
            return;

        if (directionOfLastMove != newDirection)
            Position = PositionInt;

        direction = newDirection;
    }

    private void AddTail()
    {
        GameObject newTail = new()
        {
            ForegroundColor = ForegroundColor,
            Character = Character,
        };

        GameManager.AddObject(newTail);
        tails.Add(newTail);

        speed = tails.Count * .25f + 3;
        Score = tails.Count;
        scoreText.Text = $"Score: {tails.Count}";

        const short min = (short)PianoKey.C4;
        const short max = (short)PianoKey.C6;

        var number = (Math.Sin(tails.Count / 3f) + 1) / 2;
        keys.Add((PianoKey)((max - min) * number + min));

        Note[] notes = new Note[keys.Count];
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i] = new(keys[i], Math.Max(1000 / notes.Length, 300));
        }
        Sound.PlaySound(notes);
    }

    private void UpdateTails(VectorInt lastPos)
    {
        if (tails.Count == 0)
            return;

        for (int i = tails.Count - 1; i >= 1; i--)
            tails[i].Position = tails[i - 1].Position;
        tails[0].Position = lastPos;
    }

    private bool AppleInTail()
    {
        for (int i = 0; i < tails.Count; i++)
        {
            if (apple.PositionInt == tails[i].PositionInt)
                return true;
        }

        if (apple.PositionInt == PositionInt)
            return true;
        return false;
    }

    public override void CollidedWith(GameObject other, VectorInt direction)
    {
        if (other == apple)
        {
            AddTail();

            do
            {
                apple.Position = new VectorInt(
                random.Next(0, GameWindow.ScreenDimensions.x),
                random.Next(0, GameWindow.ScreenDimensions.y)
                );
            }
            while (AppleInTail());
            
            return;
        }

        if (tails.Contains(other))
            DieManager.Die();
    }
}
