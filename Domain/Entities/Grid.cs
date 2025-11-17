namespace Domain.Entities;
public sealed class Grid
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
