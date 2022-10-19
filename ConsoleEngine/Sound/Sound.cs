using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ConsoleEngine.Sound;

public static class Sound
{
    public static bool SoundAvailable => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static void PlaySound(params Note[] sound)
    {
        Task.Factory.StartNew(() => PlaySoundThreaded(sound, null));
    }

    public static void PlaySound(Note[] sound, Action onFinished)
    {
        Task.Factory.StartNew(() => PlaySoundThreaded(sound, onFinished));
    }

    private static void PlaySoundThreaded(Note[] sound, Action? onFinished)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        for (int i = 0; i < sound.Length; i++)
        {
            if ((int)sound[i].Frequency < 37)
            {
                //delay
                Thread.Sleep(sound[i].Duration_MS);
                continue;
            }

            //cannot be above 32767, it's a short
            Console.Beep((int)sound[i].Frequency, sound[i].Duration_MS);
        }

        onFinished?.Invoke();
    }
}
