using System.Text;

namespace ConsoleEngine;

internal class Renderer
{
    public static bool DoRendering { get; set; } = true;
    internal static Queue<VectorInt> outOfBoundsClear = new();

    internal static void Render()
    {
        if (!DoRendering)
            return;

        int horizontalBarLength = GameWindow.ScreenDimensions.x;

        if (GameManager.DoubleUpCharacters)
            horizontalBarLength *= 2;

        StringBuilder horizontal = new(GameWindow.Instance.ForegroundColor.BeginColor(true));

        for (int i = 0; i < horizontalBarLength; i++)
            horizontal.Append(GameWindow.HorizontalBar);

        string endColorBorder = GameWindow.Instance.ForegroundColor.Enabled ? ColorStringConversion.END_COLOR : "";

        var screenInfo = GetScreenInformation(out ScreenAndPositionInformation[] offscreen);

        string leftScreenPadding = "";
        for (int i = 0; i < GameWindow.Instance.PositionInt.x; i++)
            leftScreenPadding += " ";

        StringBuilder sb = new();
        string side = ColorStringConversion.ToConsoleColor(
            GameWindow.VertictalBar, GameWindow.Instance.ForegroundColor, Color.None);

        bool renderLeftSide = GameWindow.Instance.PositionInt.x > 0;
        bool renderTopSide = GameWindow.Instance.PositionInt.y > 0;

        if (renderTopSide)
        {
            if (renderLeftSide)
                sb.Append(ColorStringConversion.ToConsoleColor(GameWindow.CornerTL, GameWindow.Instance.ForegroundColor, Color.None)
                    + horizontal.ToString() + GameWindow.CornerTR + endColorBorder + "\n" + leftScreenPadding);
            else
                sb.AppendLine(horizontal.ToString() + GameWindow.CornerTR + endColorBorder + "\n" + leftScreenPadding);
        }

        for (int y = 0; y < GameWindow.ScreenDimensions.y; y++)
        {
            if (renderLeftSide)
                sb.Append(side);

            for (int x = 0; x < GameWindow.ScreenDimensions.x; x++)
            {
                if (screenInfo[x, y].Character is null)
                {
                    if (GameManager.DoubleUpCharacters)
                        sb.Append(' ');

                    sb.Append(' ');
                    continue;
                }

                var character = screenInfo[x, y].Character;

                var foreground = screenInfo[x, y].Color.Foreground;
                var background = screenInfo[x, y].Color.Background;

                if (GameManager.DoubleUpCharacters)
                    sb.Append(ColorStringConversion.ToConsoleColor(character.ToString() + character, foreground, background));
                else
                    sb.Append(ColorStringConversion.ToConsoleColor(character.ToString() + "", foreground, background)); //+ "" so it won't be NULL
            }

            sb.Append(side + "\n" + leftScreenPadding);
        }

        if (renderLeftSide)
            sb.Append(ColorStringConversion.ToConsoleColor(GameWindow.CornerBL, GameWindow.Instance.ForegroundColor, Color.None)
                + horizontal + GameWindow.CornerBR + endColorBorder);
        else
            sb.Append(horizontal + GameWindow.CornerBR + endColorBorder);

        int left = GameWindow.Instance.PositionInt.x < 0 ? 0 : GameWindow.Instance.PositionInt.x;
        int top = GameWindow.Instance.PositionInt.y < 0 ? 0 : GameWindow.Instance.PositionInt.y;
        Console.SetCursorPosition(left, top);
        Console.Write(sb.ToString());

        while (outOfBoundsClear.Count > 0)
        {
            var position = outOfBoundsClear.Dequeue();
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(" ");
        }

        Dictionary<VectorInt, int> offscreen_layerByPos = new();

        for (int i = 0; i < offscreen.Length; i++)
        {
            if (offscreen_layerByPos.ContainsKey(offscreen[i].Position))
            {
                if (offscreen_layerByPos[offscreen[i].Position] > offscreen[i].ScreenInformation.Layer)
                    continue;

                offscreen_layerByPos[offscreen[i].Position] = offscreen[i].ScreenInformation.Layer;
                Console.SetCursorPosition(offscreen[i].Position.x, offscreen[i].Position.y);
                Console.Write(ColorStringConversion.ToConsoleColor(offscreen[i].ScreenInformation.Character.ToString() + "",
                    offscreen[i].ScreenInformation.Color.Foreground, offscreen[i].ScreenInformation.Color.Background));

                continue;
            }

            offscreen_layerByPos.Add(offscreen[i].Position, offscreen[i].ScreenInformation.Layer);

            if (offscreen[i].Position.x < 0 || offscreen[i].Position.y < 0)
                continue;
            Console.SetCursorPosition(offscreen[i].Position.x, offscreen[i].Position.y);

            string thing2write = ColorStringConversion.ToConsoleColor(offscreen[i].ScreenInformation.Character.ToString() + "",
                offscreen[i].ScreenInformation.Color.Foreground, offscreen[i].ScreenInformation.Color.Background);
            Console.Write(thing2write);
        }

        Console.Out.Flush();
    }

    private static bool ShouldRenderObject(ScreenInformation[,] old, GameObjectInfo info, out bool inBounds)
    {
        inBounds = info.Position.IsInBoundsScreenSpace;

        //end if not in bounderies and shouldn't render offscreen
        //not (in bounderies or offscreen)
        if (!(inBounds || info.RenderOffscreen))
            return false;

        if (!inBounds)
            return false;

        int x = info.Position.x;
        int y = info.Position.y;

        var oldObject = old[x, y];

        if (oldObject.Character is null)
            return true;

        if (oldObject.Layer > info.Layer)
            return false;

        return true;
    }

    private static ScreenInformation[,] GetScreenInformation(out ScreenAndPositionInformation[] offscreen)
    {
        ScreenInformation[,] characters = new ScreenInformation[GameWindow.ScreenDimensions.x, GameWindow.ScreenDimensions.y];
        List<ScreenAndPositionInformation> offscreenList = new();

        foreach (var gameObject in GameManager.gameObjects)
        {
            if (gameObject.Invisible || !gameObject.IsAlive)
                continue;

            if (gameObject is TextObject textObject)
            {
                RenderTextObject(ref characters, ref offscreenList, textObject);
                continue;
            }

            if (gameObject is TextureObject textureObject)
            {
                RenderTextureObject(ref characters, ref offscreenList, textureObject);
                continue;
            }

            ColorInformation colorInformation = new(gameObject.ForegroundColor, gameObject.BackgroundColor);
            GameObjectInfo gameObjectInfo = new(gameObject.Character, gameObject.Layer, gameObject.RenderPosition,
                gameObject.RenderOffscreen, colorInformation);

            if (ShouldRenderObject(characters, gameObjectInfo, out bool inBounds))
            {
                if (inBounds)
                    AddToRenderList(ref characters, gameObject);
                else
                    AddToOffscreenRenderList(ref offscreenList, gameObject);
            }
        }

        offscreen = offscreenList.ToArray();
        return characters;
    }

    private static void RenderTextObject(ref ScreenInformation[,] characters,
        ref List<ScreenAndPositionInformation> offscreenList, TextObject textObject)
    {
        for (int i = 0; i < textObject.Text.Length; i++)
        {
            int position = textObject.RenderPosition.x + i;
            bool isOutOfBounds = !new VectorInt(position, textObject.RenderPosition.y).IsInBoundsScreenSpace;

            if (isOutOfBounds)
            {
                if (!textObject.RenderOffscreen)
                    continue;
                offscreenList.Add(new ScreenAndPositionInformation(
                    new(textObject.Text[i], textObject.Layer, new(textObject.GetForegroundAt(i), textObject.GetBackgroundAt(i))),
                    textObject.PositionInt + new VectorInt(i, 0)
                    ));
                continue;
            }

            int y = textObject.RenderPosition.y;
            if (characters[position, y].Layer > textObject.Layer)
                continue;

            characters[position, y] = new ScreenInformation(
                textObject.Text[i], textObject.Layer,
                new(textObject.GetForegroundAt(i), textObject.GetBackgroundAt(i)));
        }

        for (int i = 0; i < textObject.clearCharacters; i++)
        {
            int position = textObject.PositionInt.x + textObject.Text.Length + i;
            bool isInBounds = new VectorInt(position, textObject.PositionInt.y).IsInBoundsScreenSpace;

            if (isInBounds)
                continue;

            if (!textObject.RenderOffscreen)
                continue;

            offscreenList.Add(new ScreenAndPositionInformation(
                new(' ', 0, new(Color.None, Color.None)), new VectorInt(position, textObject.PositionInt.y)));
            continue;
        }

        textObject.clearCharacters = 0;
    }

    private static void RenderTextureObject(ref ScreenInformation[,] characters,
        ref List<ScreenAndPositionInformation> offscreenList, TextureObject textureObject)
    {
        for (int y = 0; y < textureObject.Texture.Length; y++)
        {
            for (int x = 0; x < textureObject.Texture[y].Length; x++)
            {
                if (textureObject.Texture[y][x] == '\0')
                    continue;
                VectorInt position = textureObject.RenderPosition + new VectorInt(x, y);
                ColorInformation colorInformation = new(textureObject.GetForegroundAt(x, y), textureObject.GetBackgroundAt(x, y));

                if (position.IsInBoundsScreenSpace)
                {
                    if (characters[position.x, position.y].Layer > textureObject.Layer)
                        continue;
                    characters[position.x, position.y] = new ScreenInformation(textureObject.Texture[y][x], textureObject.Layer, colorInformation);
                }

                if (!textureObject.RenderOffscreen)
                    continue;

                VectorInt offscreenPos = textureObject.PositionInt + new VectorInt(x, y);
                offscreenList.Add(new ScreenAndPositionInformation(new(textureObject.Texture[y][x], textureObject.Layer,
                    colorInformation), offscreenPos));
            }
        }
    }

    private static void AddToRenderList(ref ScreenInformation[,] characters, GameObject gameObject)
    {
        int x = gameObject.RenderPosition.x;
        int y = gameObject.RenderPosition.y;

        bool doubleUp = GameManager.DoubleUpCharacters;

        if (gameObject is TextObject)
            doubleUp = false;
        else if (gameObject is TextureObject textureObject)
            doubleUp = textureObject.DoubleUp && GameManager.DoubleUpCharacters;

        characters[x, y] = new ScreenInformation(
            gameObject.Character, gameObject.Layer, new(gameObject.ForegroundColor, gameObject.BackgroundColor));
    }

    private static void AddToOffscreenRenderList(ref List<ScreenAndPositionInformation> offscreenList, GameObject gameObject)
    {
        if (gameObject.PositionInt.x < 0 || gameObject.PositionInt.y < 0)
            return;

        ColorInformation color = new(gameObject.ForegroundColor, gameObject.BackgroundColor);

        ScreenInformation screenInformation = new(gameObject.Character, gameObject.Layer, color);

        ScreenAndPositionInformation data = new(screenInformation, gameObject.PositionInt);

        offscreenList.Add(data);
    }

    record struct ScreenInformation(char? Character, int Layer, ColorInformation Color);
    record struct ScreenAndPositionInformation(ScreenInformation ScreenInformation, VectorInt Position);
    record struct GameObjectInfo(char Character, int Layer, VectorInt Position, bool RenderOffscreen, ColorInformation ColorInformation);
    record struct ColorInformation(Color Foreground, Color Background);
}