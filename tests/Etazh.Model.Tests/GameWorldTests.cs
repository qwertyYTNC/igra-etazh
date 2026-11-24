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

    [Fact]
    public void ПрицелВправо_МеняетНаправлениеИНеДвигаетИгрока()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 100, Y = 120 });
        world.Pistol = new Pistol { X = 10, Y = 10, Ammo = 6 };
        var centerX = world.Player.X + Player.Size / 2;
        var centerY = world.Player.Y + Player.Size / 2;

        world.AimAt(centerX + 80, centerY);

        Assert.Equal(1, world.Player.AimX, 3);
        Assert.Equal(0, world.Player.AimY, 3);
        Assert.Equal(100, world.Player.X);
        Assert.Equal(120, world.Player.Y);
        Assert.Equal(6, world.Pistol.Ammo);
        Assert.False(world.Pistol.PickedUp);
    }

    [Fact]
    public void ПрицелВверх_СмотритВверх()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 100, Y = 120 });
        var centerX = world.Player.X + Player.Size / 2;
        var centerY = world.Player.Y + Player.Size / 2;

        world.AimAt(centerX, centerY - 50);

        Assert.Equal(0, world.Player.AimX, 3);
        Assert.Equal(-1, world.Player.AimY, 3);
        Assert.Equal(100, world.Player.X);
        Assert.Equal(120, world.Player.Y);
    }

    [Fact]
    public void ДальнийПротивник_СтоитНаМесте()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 50, Y = 50 });
        world.Enemies.Add(new Enemy { X = 700, Y = 500 });

        world.Step();

        Assert.Equal(700, world.Enemies[0].X);
        Assert.Equal(500, world.Enemies[0].Y);
    }

    [Fact]
    public void БлижнийПротивник_ДелаетШагКИгроку()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 200, Y = 200 });
        world.Enemies.Add(new Enemy { X = 200, Y = 280 });

        world.Step();

        Assert.Equal(200, world.Enemies[0].X);
        Assert.Equal(280 - Enemy.Speed, world.Enemies[0].Y);
        Assert.Equal(200, world.Player.X);
        Assert.Equal(200, world.Player.Y);
    }

    [Fact]
    public void ПротивникУЛевойСтены_НеВыходитЗаКомнату()
    {
        var world = new GameWorld(new Room(800, 600), new Player { X = 0, Y = 100 });
        world.Enemies.Add(new Enemy { X = 1, Y = 100 });

        world.Step();

        Assert.Equal(0, world.Enemies[0].X);
        Assert.InRange(world.Enemies[0].X, 0, world.Room.Width - Enemy.Size);
    }

    [Fact]
    public void СтартоваяКомната_ПятьПротивниковСтоятВдали()
    {
        var world = GameWorld.CreateStarting();
        Assert.Equal(5, world.Enemies.Count);
        var before = world.Enemies.Select(enemy => (enemy.X, enemy.Y)).ToArray();

        world.Step();

        for (var i = 0; i < before.Length; i++)
        {
            Assert.Equal(before[i].X, world.Enemies[i].X);
            Assert.Equal(before[i].Y, world.Enemies[i].Y);
        }
    }
}
