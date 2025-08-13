using System;
using System.Threading;
// using Life;
using Treee;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;

class Program
{
    public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace()
                         .UseReactiveUI();
    
    
    static void Main(string[] args)
{

    BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);

    // var sim = new QuadTree(4);

    // sim.SetAlive(0, 0);
    // sim.SetAlive(0, 1);
    // sim.SetAlive(1, 0);
    // sim.SetAlive(-1, 0);
    // sim.SetAlive(0, -1); 

    // while (true)
    // {
    //     Console.Clear();
    //     sim.PrintToConsole(20, 20);
    //     sim.Step();
    //     Thread.Sleep(100);
    // }
}
}
