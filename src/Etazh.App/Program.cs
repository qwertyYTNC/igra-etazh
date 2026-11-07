using Etazh.Model;

namespace Etazh.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var room = new Room(800, 600);
        var player = new Player
        {
            X = (room.Width - Player.Size) / 2,
            Y = (room.Height - Player.Size) / 2
        };
        var world = new GameWorld(room, player);
        Application.Run(new GameForm(world));
    }
}
