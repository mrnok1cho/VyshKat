namespace VyshKat.App.Domain.Contracts;

public interface IFleetInventory
{
    IReadOnlyCollection<IInventoryItem> Assets { get; }
    bool ContainsInventoryNumber(string inventoryNumber);
    void Add(IInventoryItem item);
}
