namespace ConsoleEngine;

public class TextObject : GameObject
{
    public bool EnableFormatting { get; set; }
    private string _text = "";
    private Color[] Foreground = Array.Empty<Color>();
    private Color[] Background = Array.Empty<Color>();

    public string Text
    {
        get => _text;
        set
        {
            string inputValue = value;
            if (EnableFormatting)
                inputValue = ParseFormatting(value);
            clearCharacters = inputValue.Length < _text.Length ? _text.Length - inputValue.Length : 0;
            _text = inputValue;
        }
    }
    internal int clearCharacters = 0;

    public Color GetForegroundAt(int i)
    {
        return Foreground.Length <= i ? Color.None : Foreground[i];
    }

    public Color GetBackgroundAt(int i)
    {
        return Background.Length <= i ? Color.None : Background[i];
    }

    private string ParseFormatting(string input)
    {
        string newValue = "";
        
        List<Color> foreground = new(_text.Length);
        List<Color> background = new(_text.Length);

        Color currentForeground = Color.None, currentBackground = Color.None;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '#')
            {
                if (input[i + 1] == '#')
                {
                    i++;
                    newValue += "#";
                    continue;
                }

                if (input[i + 1] == 'f')
                    currentForeground = Color.Parse(input.Substring(i + 2, 6));
                else if (input[i + 1] == 'b')
                    currentBackground = Color.Parse(input.Substring(i + 2, 6));
                else
                {
                    currentBackground = Color.Parse(input.Substring(i + 1, 6));
                    i += 7;
                    continue;
                }

                i += 7;
                continue;
            }

            newValue += input[i];
            foreground.Add(currentForeground);
            background.Add(currentBackground);
        }

        Foreground = foreground.ToArray();
        Background = background.ToArray();

        return newValue;
    }
}
