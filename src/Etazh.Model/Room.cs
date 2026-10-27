namespace Etazh.Model;

public sealed class Room
{
    public Room(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double Width { get; }

    public double Height { get; }
}
