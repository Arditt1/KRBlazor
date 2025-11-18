using Domain.Entities;
using Domain.Interface;

namespace Infrastructure.State;

public class AnchorageState : IAnchorageState
{
    private readonly List<ShipSpecification> _specs = [];
    private readonly List<PlacedShip> _placed = [];

    public bool AllVesselsPlaced =>
        Specs.Any() &&
        Specs.All(spec => Placed.Count(p => p.Designation == spec.Designation) >= spec.Count);

    public Grid Grid { get; private set; } = new(0, 0);
    public IReadOnlyList<ShipSpecification> Specs => _specs;
    public IReadOnlyList<PlacedShip> Placed => _placed;

    public event Action? OnChange;
    private void Notify() => OnChange?.Invoke();

    public void Reset(Grid grid, IEnumerable<ShipSpecification> specs)
    {
        Grid = grid;
        _specs.Clear();
        _specs.AddRange(specs);
        _placed.Clear();
        Notify();
    }

    public bool TryAdd(PlacedShip ship)
    {
        ClampWithinBounds(ship);
        if (!InBounds(ship) || Overlaps(ship))
            return false;

        _placed.Add(ship);
        Notify();
        return true;
    }

    public bool TryMove(Guid id, int x, int y)
    {
        PlacedShip? s = _placed.FirstOrDefault(p => p.Id == id);
        if (s == null)
            return false;

        (int oldX, int oldY) = (s.X, s.Y);
        s.X = x;
        s.Y = y;

        ClampWithinBounds(s);

        if (!InBounds(s) || Overlaps(s, s.Id))
        {
            s.X = oldX;
            s.Y = oldY;
            return false;
        }

        Notify();
        return true;
    }


    public bool TryRotate(Guid id)
    {
        var s = _placed.FirstOrDefault(p => p.Id == id);
        if (s == null) return false;

        var oldW = s.Width;
        var oldH = s.Height;
        var oldX = s.X;
        var oldY = s.Y;

        s.Rotate90();

        if (!InBounds(s) || Overlaps(s, s.Id))
        {
            s.Width = oldW;
            s.Height = oldH;
            s.X = oldX;
            s.Y = oldY;
            return false;
        }

        ClampWithinBounds(s);
        Notify();
        return true;
    }

    public void Remove(Guid id)
    {
        _placed.RemoveAll(p => p.Id == id);
        Notify();
    }

    public bool InBounds(PlacedShip s)
    {
        return s.X >= 0 && s.Y >= 0 &&
               s.X + s.Width <= Grid.Width &&
               s.Y + s.Height <= Grid.Height;
    }

    private bool ClampWithinBounds(PlacedShip s)
    {
        if (Grid.Width <= 0 || Grid.Height <= 0)
            return false;

        if (s.Width > Grid.Width || s.Height > Grid.Height)
            return false;

        if (s.X < 0)
            s.X = 0;

        if (s.Y < 0)
            s.Y = 0;

        if (s.X + s.Width > Grid.Width)
            s.X = Grid.Width - s.Width;

        if (s.Y + s.Height > Grid.Height)
            s.Y = Grid.Height - s.Height;

        return true;
    }

    public bool Overlaps(PlacedShip s, Guid? ignore = null)
    {
        return _placed.Any(p => p.Id != ignore &&
            p.X < s.X + s.Width && s.X < p.X + p.Width &&
            p.Y < s.Y + s.Height && s.Y < p.Y + p.Height);
    }
}
