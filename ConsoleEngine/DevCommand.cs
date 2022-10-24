namespace ConsoleEngine;

public class DevCommand
{
    internal static Dictionary<string, DevCommand> commands = new();
    public static DevCommand[] DevCommands => commands.Values.ToArray();

    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public Func<string[], string> Action { get; init; }
    private bool builtIn;

    public DevCommand(string name, string description, Func<string[], string> action)
    {
        Name = name;
        Description = description;
        Action = action;
    }

    internal DevCommand(string name, string description, bool builtIn, Func<string[], string> action)
    {
        Name = name;
        Description = description;
        Action = action;
        this.builtIn = builtIn;
    }

    public void Register()
    {
        if (!commands.ContainsKey(Name))
            commands.Add(Name, this);
    }

    public void Unregister()
    {
        if (builtIn)
            return;
        if (commands.ContainsKey(Name))
            commands.Remove(Name);
    }

    public override string ToString()
    {
        return $"{Name} - {Description}";
    }

    public static void UnregisterAll()
    {
        foreach (var command in commands)
        {
            command.Value.Unregister();
        }
    }
}
