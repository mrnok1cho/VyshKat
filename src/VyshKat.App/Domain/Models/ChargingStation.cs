using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Domain.Models;

public sealed class ChargingStation : IInventoryItem, IEnergyConsumer
{
    public ChargingStation(string inventoryNumber, decimal dailyEnergyConsumptionKwh)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Инвентарный номер обязателен.", nameof(inventoryNumber));
        if (dailyEnergyConsumptionKwh < 0)
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyConsumptionKwh), "Расход энергии не может быть отрицательным.");
        Name = "Зарядная станция";
        InventoryNumber = inventoryNumber.Trim();
        DailyEnergyConsumptionKwh = dailyEnergyConsumptionKwh;
    }

    public string Name { get; }
    public string InventoryNumber { get; }
    public decimal DailyEnergyConsumptionKwh { get; }
}
