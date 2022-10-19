using System.Runtime.InteropServices;

namespace ConsoleEngine;

internal static class ConsoleColorManager
{
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    public static void Enable()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            Color.ColorsAllowed = GetConsoleMode(iStdOut, out var outConsoleMode)
                         && SetConsoleMode(iStdOut, outConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }


        Color.ColorsAllowed = Environment.GetEnvironmentVariable("NO_COLOR") == null;
    }
}
