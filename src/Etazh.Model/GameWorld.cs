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

    private int _horizontal;

    private int _vertical;

    public void SetHorizontalMove(int direction)
    {
        _horizontal = Math.Clamp(direction, -1, 1);
    }

    public void SetVerticalMove(int direction)
    {
        _vertical = Math.Clamp(direction, -1, 1);
    }

    public void Step()
    {
        Player.X = Clamp(Player.X + _horizontal * Player.Speed, 0, Room.Width - Player.Size);
        Player.Y = Clamp(Player.Y + _vertical * Player.Speed, 0, Room.Height - Player.Size);
    }

    private static double Clamp(double value, double min, double max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}
