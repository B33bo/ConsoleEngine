namespace ConsoleEngine.Sound;

public struct Note
{
    public PianoKey Frequency;
    public int Duration_MS;

    public Note(PianoKey key, int durationMS)
    {
        Frequency = key;
        Duration_MS = durationMS;
    }

    public static Note Parse(string input, int defaultOctive)
    {
        if (TryParse(input, defaultOctive, out Note note))
            return note;
        throw new FormatException("Input string was not in a correct format.");
    }

    public static bool TryParse(string input, int defaultOctive, out Note result)
    {
        if (input == String.Empty || input == " ")
        {
            result = new(0, 0);
            return true;
        }

        input = input.ToUpper();

        if (int.TryParse(input, out int freq))
        {
            result = new((PianoKey)freq, 200);
            return true;
        }

        string key = input[0].ToString();
        if (input.Length == 1)
        {
            if (!Enum.TryParse(key + defaultOctive, out PianoKey pianoKey))
            {
                result = new(0, 0);
                return false;
            }

            result = new(pianoKey, 200);
            return true;
        }

        if (input[1] == '#')
            key += "sharp";
        else if (input[1] == 'B' || input[1] == '♭')
        {
            if (key == "A")
                key = "G";
            else
                key = ((char)(key[0] - 1)).ToString();

            key += "sharp";

            if (input.Length == 2)
            {
                if (!Enum.TryParse(key + defaultOctive, out PianoKey pianoKey))
                {
                    result = new(0, 0);
                    return false;
                }

                result = new(pianoKey, 200);
                return true;
            }
        }

        if (key.EndsWith("sharp"))
        {
            if (!Enum.TryParse(key + defaultOctive, out PianoKey pianoKey))
            {
                result = new(0, 0);
                return false;
            }

            result = new(pianoKey, 200);
            return true;
        }

        if (Enum.TryParse(key + input[key.Length..], out PianoKey keyResult))
        {
            result = new(keyResult, 200);
            return true;
        }

        result = new(0, 0);
        return false;
    }

    public static Note[] Parse(string input, string beats, int lengthPerBeats)
    {
        if (TryParse(input, beats, lengthPerBeats, out Note[] result))
            return result;
        throw new FormatException("Input string was not in a correct format.");
    }

    public static bool TryParse(string notes, string beats, int lengthPerBeats, out Note[] result)
    {
        result = Array.Empty<Note>();

        string[] notesArray = notes.Split('/');
        string[] beatsArray = beats.Split('/');

        for (int i = 0; i < notesArray.Length; i++)
        {
            if (!TryParse(notesArray[i], 4, out Note note))
                return false;
            if (!int.TryParse(beatsArray[i], out int beat))
                return false;
            note.Duration_MS = beat * lengthPerBeats;
            result[i] = note;
        }

        return true;
    }
}
