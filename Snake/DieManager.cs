using ConsoleEngine;

namespace Snake;

internal class DieManager
{
    public static void Die()
    {
        var objects = GameManager.GameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Destroy();
        }

        TextureObject gameOverText = new()
        {
            TextureString = $"GAME OVER!\nYOU SCORED: {Snake.Score}",
            RenderOffscreen = true,
        };

        GameWindow.ScreenDimensions = new(0, 0);
        GameManager.AddObject(gameOverText);
    }
}
