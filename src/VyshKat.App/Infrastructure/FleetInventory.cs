using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Infrastructure;

public sealed class FleetInventory : IFleetInventory
{
    private readonly List<IInventoryItem> _assets = [];

    public IReadOnlyCollection<IInventoryItem> Assets => _assets.AsReadOnly();

    public bool ContainsInventoryNumber(string inventoryNumber) =>
        _assets.Any(asset => string.Equals(asset.InventoryNumber, inventoryNumber, StringComparison.OrdinalIgnoreCase));

    public void Add(IInventoryItem item)
    {
        if (ContainsInventoryNumber(item.InventoryNumber))
            throw new InvalidOperationException($"Инвентарный номер {item.InventoryNumber} уже используется.");
        _assets.Add(item);
    }
}
