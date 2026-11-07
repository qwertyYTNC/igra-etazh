using Etazh.Model;

namespace Etazh.Model.Tests;

public class GameWorldTests
{
    [Fact]
    public void ПустойШаг_НеМеняетСостояниеКомнаты()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 100, Y = 120 });
        world.Enemies.Add(new Enemy { X = 400, Y = 300 });
        world.Bullets.Add(new Bullet { X = 10, Y = 20 });
        world.Pistol = new Pistol { X = 50, Y = 60, Ammo = 6 };

        var roomWidth = world.Room.Width;
        var roomHeight = world.Room.Height;
        var playerX = world.Player.X;
        var playerY = world.Player.Y;
        var enemyX = world.Enemies[0].X;
        var enemyY = world.Enemies[0].Y;
        var bulletX = world.Bullets[0].X;
        var bulletY = world.Bullets[0].Y;
        var pistolX = world.Pistol.X;
        var pistolY = world.Pistol.Y;
        var pistolAmmo = world.Pistol.Ammo;
        var pistolPickedUp = world.Pistol.PickedUp;

        world.Step();

        Assert.Equal(roomWidth, world.Room.Width);
        Assert.Equal(roomHeight, world.Room.Height);
        Assert.Equal(playerX, world.Player.X);
        Assert.Equal(playerY, world.Player.Y);
        Assert.Equal(enemyX, world.Enemies[0].X);
        Assert.Equal(enemyY, world.Enemies[0].Y);
        Assert.Equal(bulletX, world.Bullets[0].X);
        Assert.Equal(bulletY, world.Bullets[0].Y);
        Assert.Equal(pistolX, world.Pistol.X);
        Assert.Equal(pistolY, world.Pistol.Y);
        Assert.Equal(pistolAmmo, world.Pistol.Ammo);
        Assert.Equal(pistolPickedUp, world.Pistol.PickedUp);
        Assert.Single(world.Enemies);
        Assert.Single(world.Bullets);
    }

    [Fact]
    public void ШагВправо_СдвигаетИгрока()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 100, Y = 120 });
        world.SetHorizontalMove(1);

        world.Step();

        Assert.Equal(100 + Player.Speed, world.Player.X);
        Assert.Equal(120, world.Player.Y);
    }

    [Fact]
    public void ШагБезКоманды_НеМеняетКоординатыИгрока()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 100, Y = 120 });

        world.Step();

        Assert.Equal(100, world.Player.X);
        Assert.Equal(120, world.Player.Y);
    }

    [Fact]
    public void ШагВПравуюСтену_ОстанавливаетИгрока()
    {
        var room = new Room(800, 600);
        var maxX = room.Width - Player.Size;
        var world = new GameWorld(room, new Player { X = maxX - 1, Y = 50 });
        world.SetHorizontalMove(1);

        world.Step();

        Assert.Equal(maxX, world.Player.X);
    }

    [Fact]
    public void ШагВЛевуюСтену_ОстанавливаетИгрока()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 1, Y = 50 });
        world.SetHorizontalMove(-1);

        world.Step();

        Assert.Equal(0, world.Player.X);
    }

    [Fact]
    public void ШагВВерхнююСтену_ОстанавливаетИгрока()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 50, Y = 1 });
        world.SetVerticalMove(-1);

        world.Step();

        Assert.Equal(0, world.Player.Y);
    }

    [Fact]
    public void ШагВНижнююСтену_ОстанавливаетИгрока()
    {
        var room = new Room(800, 600);
        var maxY = room.Height - Player.Size;
        var world = new GameWorld(room, new Player { X = 50, Y = maxY - 1 });
        world.SetVerticalMove(1);

        world.Step();

        Assert.Equal(maxY, world.Player.Y);
    }
}
