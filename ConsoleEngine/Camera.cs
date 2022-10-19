namespace ConsoleEngine;

public class Camera : GameObject
{
    public static Camera? MainCamera { get; set; }

    public Camera()
    {
        MainCamera ??= this;
        Character = '@';
        Invisible = true;
        CollisionEnabled = false;
        Position = new(0, 0);
    }
}
