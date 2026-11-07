using Etazh.Model;

namespace Etazh.App;

public sealed class GameForm : Form
{
    private readonly GameWorld _world;
    private readonly System.Windows.Forms.Timer _timer;
    private readonly HashSet<Keys> _heldKeys = new();

    public GameForm(GameWorld world)
    {
        _world = world;
        Text = "Этаж";
        ClientSize = new Size((int)world.Room.Width, (int)world.Room.Height);
        DoubleBuffered = true;
        KeyPreview = true;
        BackColor = Color.FromArgb(32, 32, 36);

        _timer = new System.Windows.Forms.Timer { Interval = 30 };
        _timer.Tick += OnTick;
        _timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var room = _world.Room;
        var bounds = new Rectangle(2, 2, (int)room.Width - 4, (int)room.Height - 4);
        using var floor = new SolidBrush(Color.FromArgb(48, 46, 52));
        using var wall = new Pen(Color.FromArgb(180, 170, 150), 4);
        e.Graphics.FillRectangle(floor, bounds);
        e.Graphics.DrawRectangle(wall, bounds);

        var player = _world.Player;
        using var body = new SolidBrush(Color.FromArgb(214, 176, 122));
        e.Graphics.FillEllipse(
            body,
            (float)player.X,
            (float)player.Y,
            (float)Player.Size,
            (float)Player.Size);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode is not (Keys.W or Keys.A or Keys.S or Keys.D))
        {
            return;
        }

        _heldKeys.Add(e.KeyCode);
        ApplyMove();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (_heldKeys.Remove(e.KeyCode))
        {
            ApplyMove();
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        _timer.Dispose();
        base.OnFormClosed(e);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        _world.Step();
        Invalidate();
    }

    private void ApplyMove()
    {
        var horizontal = (_heldKeys.Contains(Keys.D) ? 1 : 0) - (_heldKeys.Contains(Keys.A) ? 1 : 0);
        var vertical = (_heldKeys.Contains(Keys.S) ? 1 : 0) - (_heldKeys.Contains(Keys.W) ? 1 : 0);
        _world.SetHorizontalMove(horizontal);
        _world.SetVerticalMove(vertical);
    }
}
