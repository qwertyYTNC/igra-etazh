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

    public void AimAt(double x, double y)
    {
        var dx = x - (Player.X + Player.Size / 2);
        var dy = y - (Player.Y + Player.Size / 2);
        var length = Math.Sqrt(dx * dx + dy * dy);
        if (length == 0)
        {
            return;
        }

        Player.AimX = dx / length;
        Player.AimY = dy / length;
    }

    public static GameWorld CreateStarting()
    {
        var room = new Room(800, 600);
        var player = new Player
        {
            X = (room.Width - Player.Size) / 2,
            Y = (room.Height - Player.Size) / 2
        };
        var world = new GameWorld(room, player);
        world.Enemies.Add(new Enemy { X = 30, Y = 30 });
        world.Enemies.Add(new Enemy { X = 740, Y = 30 });
        world.Enemies.Add(new Enemy { X = 30, Y = 540 });
        world.Enemies.Add(new Enemy { X = 740, Y = 540 });
        world.Enemies.Add(new Enemy { X = 386, Y = 30 });
        return world;
    }

    public void Step()
    {
        Player.X = Clamp(Player.X + _horizontal * Player.Speed, 0, Room.Width - Player.Size);
        Player.Y = Clamp(Player.Y + _vertical * Player.Speed, 0, Room.Height - Player.Size);

        foreach (var enemy in Enemies)
        {
            MoveEnemy(enemy);
        }
    }

    private void MoveEnemy(Enemy enemy)
    {
        var playerX = Player.X + Player.Size / 2;
        var playerY = Player.Y + Player.Size / 2;
        var enemyX = enemy.X + Enemy.Size / 2;
        var enemyY = enemy.Y + Enemy.Size / 2;
        var dx = playerX - enemyX;
        var dy = playerY - enemyY;
        var distance = Math.Sqrt(dx * dx + dy * dy);
        if (distance > Enemy.DetectionRadius || distance == 0)
        {
            return;
        }

        var step = Math.Min(Enemy.Speed, distance);
        enemy.X = Clamp(enemy.X + dx / distance * step, 0, Room.Width - Enemy.Size);
        enemy.Y = Clamp(enemy.Y + dy / distance * step, 0, Room.Height - Enemy.Size);
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
