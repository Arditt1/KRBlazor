namespace Domain.Entities;
public sealed class PlacedShip
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Designation { get; set; } = string.Empty;
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public PlacedShip() { }

    public void Rotate90()
    {
        (Width, Height) = (Height, Width);
    }
}
