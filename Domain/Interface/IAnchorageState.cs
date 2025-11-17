using Domain.Entities;

namespace Domain.Interface;

public interface IAnchorageState
{
    event Action? OnChange;
    Grid Grid { get; }
    IReadOnlyList<ShipSpecification> Specs { get; }
    IReadOnlyList<PlacedShip> Placed { get; }
    bool AllVesselsPlaced { get; }

    void Reset(Grid grid, IEnumerable<ShipSpecification> specs);
    bool TryAdd(PlacedShip ship);
    bool TryMove(Guid id, int x, int y);
    bool TryRotate(Guid id);
    void Remove(Guid id);
}
