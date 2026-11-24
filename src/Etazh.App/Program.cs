using Etazh.Model;

namespace Etazh.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new GameForm(GameWorld.CreateStarting()));
    }
}
