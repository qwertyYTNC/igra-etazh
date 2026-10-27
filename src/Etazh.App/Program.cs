using Etazh.Model;

namespace Etazh.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var world = new GameWorld(new Room(800, 600), new Player());
        Application.Run(new GameForm(world));
    }
}
