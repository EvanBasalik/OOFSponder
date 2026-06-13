using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;

namespace OOFSponderMac;

class Program
{
    internal static string AppName = "OOFSponder";
    internal static string AppDataFolder = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

    [STAThread]
    public static void Main(string[] args)
    {
        // Ensure data folder exists before logging starts
        if (!System.IO.Directory.Exists(AppDataFolder))
        {
            System.IO.Directory.CreateDirectory(AppDataFolder);
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
