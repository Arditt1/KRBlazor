namespace Domain.Entities;

public sealed class ShipSpecification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Designation { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public int Count { get; set; }

    public ShipSpecification() { }

}
