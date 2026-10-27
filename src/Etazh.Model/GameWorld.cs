namespace Etazh.Model;

public sealed class GameWorld
{
    public GameWorld(Room room, Player player)
    {
        Room = room;
        Player = player;
    }

    public Room Room { get; }

    public Player Player { get; }

    public List<Enemy> Enemies { get; } = new();

    public List<Bullet> Bullets { get; } = new();

    public Pistol? Pistol { get; set; }

    public void Step()
    {
    }
}
