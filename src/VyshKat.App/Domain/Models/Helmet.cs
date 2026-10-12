using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Domain.Models;

public sealed class Helmet : IInventoryItem
{
    public Helmet(string inventoryNumber)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Инвентарный номер обязателен.", nameof(inventoryNumber));
        Name = "Шлем";
        InventoryNumber = inventoryNumber.Trim();
    }

    public string Name { get; }
    public string InventoryNumber { get; }
}
