using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine;

internal static class Input
{
    internal static bool askingQuestion;
    private static StringBuilder answer = new();
    private static VectorInt questionOrigin;
    private static int? maxQuestionLength;
    internal static string GetAnswer => answer.ToString();

    internal static void GetKey()
    {
        var keyPressed = Console.ReadKey(true);

        if (askingQuestion)
        {
            HandleQuestion(keyPressed);
            return;
        }

        GameManager.KeyPressed = keyPressed.Key;
    }

    internal static void GetKeyForever()
    {
        while (true)
        {
            GetKey();
        }
    }

    private static void HandleQuestion(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            default:
                if (maxQuestionLength.HasValue && maxQuestionLength.Value < answer.Length)
                {
                    if (maxQuestionLength.Value <= 0)
                        break;
                    break;
                }
                answer.Append(keyInfo.KeyChar);
                Console.SetCursorPosition(questionOrigin.x, questionOrigin.y);
                Console.Write(answer.ToString());
                break;
            case ConsoleKey.Enter:
                StringBuilder blank = new();
                for (int i = 0; i < answer.Length; i++)
                    blank.Append(' ');

                Console.SetCursorPosition(questionOrigin.x, questionOrigin.y);
                Console.Write(blank);
                askingQuestion = false;
                break;
            case ConsoleKey.Backspace:
                if (Console.CursorLeft == questionOrigin.x)
                    break;
                if (answer.Length == 0)
                    break;
                Console.CursorLeft--;
                Console.Write(" ");
                Console.CursorLeft--;
                answer.Remove(answer.Length - 1, 1);
                break;
        }
    }

    internal static void AskQuestion(VectorInt origin, int? maxQuestionLength)
    {
        if (askingQuestion)
            throw new InvalidOperationException("Already asking a question!");

        answer = new StringBuilder();
        questionOrigin = origin;
        Input.maxQuestionLength = maxQuestionLength;
        askingQuestion = true; //must be (second) last
        Console.SetCursorPosition(origin.x, origin.y);
    }
}
