using ConsoleEngine.Sound;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleEngine;

public static class DevConsole
{
    private static StringBuilder commandCallback = new("Dev Console\nType 'end' to end.\n");
    private static bool didLoad = false;

    public static void ReadCommand()
    {
        Console.Clear();

        if (!didLoad)
            LoadBuiltinCommands();

        PerformCommand();
    }

    private static void PerformCommand()
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = true;
        Console.Write(commandCallback.ToString());

        GameManager.DoRendering = false;

        Console.Write("> ");
        string? input = GameManager.AskQuestion(new VectorInt(Console.CursorLeft, Console.CursorTop));

        if (input is null)
            return;

        commandCallback.AppendLine("> " + input);

        string[] args = input.Split(',');

        if (DevCommand.commands.ContainsKey(args[0]))
            commandCallback.AppendLine(DevCommand.commands[args[0]].Action.Invoke(args));
        else
            commandCallback.AppendLine("Command not found.");

        if (input == "end")
        {
            Console.Clear();
            GameManager.DoRendering = true;
            Console.CursorVisible = false;
            return;
        }
        if (input == "clr" || input == "clear" || input == "cls")
        {
            commandCallback.Clear();
            Console.Clear();
        }

        if (input != "exit")
            PerformCommand();
    }

    public static void LoadBuiltinCommands()
    {
        didLoad = true;

        new DevCommand("", "", true, _ => "").Register();
        new DevCommand("clear", "clears dev console", true, _ => "").Register();

        new DevCommand("help", "lists all of the commands and their descriptions", true, args =>
        {
            if (args.Length > 1)
            {
                if (!DevCommand.commands.ContainsKey(args[1]))
                    return "that is not a command.";

                var output = DevCommand.commands[args[1]].ToString();
                output ??= "unknown";
                return output;
            }

            string str = "list of commands. All parameters are seperated by a comma (,)\n";
            var commands = DevCommand.DevCommands;
            for (int i = 0; i < commands.Length; i++)
                str += commands[i].ToString() + "\n";

            return str;
        }).Register();

        new DevCommand("end", "ends the dev cosnole session", true, _ => "").Register();

        new DevCommand("renderingEnabled", "turns rendering on/off", true, args =>
        {
            if (args[1].ToLower() == "false")
                GameManager.DoRendering = false;

            if (args[1].ToLower() == "true")
                GameManager.DoRendering = true;

            return GameManager.DoRendering.ToString();
        }).Register();

        new DevCommand("refresh", "refreshes the screen", true, args =>
        {
            GameManager.ScreenRefresh();
            return "";
        }).Register();

        new DevCommand("maxfps", "change the max fps", true, args =>
        {
            if (args.Length == 1)
                return GameManager.MaxFPS.ToString();
            if (int.TryParse(args[1], out int result))
                GameManager.MaxFPS = result;
            else
                return "please enter a valid integer";

            return "";
        }).Register();

        new DevCommand("screenSize", "change the screen size", true, args =>
        {
            if (args.Length < 3)
                return GameWindow.ScreenDimensions.ToString();

            if (!int.TryParse(args[1], out int x))
                return "please enter a valid integer";

            if (!int.TryParse(args[1], out int y))
                return "please enter a valid integer";

            if (x < 0 || y < 0)
                return "screen dimensions cannot be less than 0";

            GameWindow.ScreenDimensions = new(x, y);
            return "";
        }).Register();

        new DevCommand("title", "change the title", true, args =>
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "?";

            if (args.Length > 1)
            {
                Console.Title = args[1];
                return "";
            }

            return Console.Title;
        }).Register();

        new DevCommand("exit", "quits the game", true, _ =>
        {
            GameManager.Stop();
            return "";
        }).Register();

        new DevCommand("haltAndCatchFire", "exits the game via fatal exception", true, _ =>
        {
            throw new Exception();
        }).Register();
    }
}
