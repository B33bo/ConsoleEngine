using System;

namespace ConsoleEngine;

public class GameObject
{
    private Vector _position;
    internal bool IsAlive { get; private set; } = true;
    public Vector Position
    {
        get => _position;
        set
        {
            if (value == _position)
                return;

            VectorInt vectorInt = (VectorInt)value;
            bool setPosition = true;

            var gameobjects = GameManager.GameObjects;

            foreach (var gameObj in gameobjects)
            {
                if (gameObj.PositionInt != vectorInt)
                    continue;
                if (gameObj == this)
                    continue;
                
                CollidedWith(gameObj, gameObj.PositionInt - PositionInt);
                gameObj.CollidedWith(this, PositionInt - gameObj.PositionInt);

                if (CollisionEnabled && gameObj.CollisionEnabled)
                    setPosition = false;
            }

            if (!setPosition)
                return;

            if (!_position.IsInBoundsScreenSpace && _position.x > 0 && _position.y > 0 && RenderOffscreen && _position.x < 128 && _position.y < 128)
                Renderer.outOfBoundsClear.Enqueue(PositionInt);

            _position = value;
        }
    }
    public VectorInt PositionInt => (VectorInt)Position;
    public char Character { get; set; } = ' ';
    public bool Invisible { get; set; }
    public byte Layer { get; set; }
    public bool RenderOffscreen { get; set; } = false;
    public bool CollisionEnabled { get; set; } = false;
    public Color ForegroundColor { get; set; }
    public Color BackgroundColor { get; set; }
    public VectorInt RenderPosition => PositionInt - (Camera.MainCamera is null ? VectorInt.Zero : Camera.MainCamera.PositionInt);

    public void Destroy()
    {
        if (this is GameWindow)
            return;
        IsAlive = false;
        OnDestroy();
    }

    public virtual void Update(double deltaTime)
    {

    }

    public virtual void KeyPressed(ConsoleKey key)
    {

    }

    public virtual void CollidedWith(GameObject other, VectorInt direction)
    {

    }

    public virtual void OnDestroy()
    {

    }
}
