namespace ConsoleEngine;

public class FPSreader : TextObject
{
    public override void Update(double deltaTime)
    {
        Text = $"FPS: {GameManager.FPS}";
    }
}
