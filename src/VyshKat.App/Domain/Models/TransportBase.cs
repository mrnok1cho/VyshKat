using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Domain.Models;

public abstract class TransportBase : ITransport
{
    protected TransportBase(
        string name,
        string inventoryNumber,
        decimal dailyEnergyConsumptionKwh,
        int beginnerFriendlyScore,
        bool isTechnicallySound)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название обязательно.", nameof(name));
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Инвентарный номер обязателен.", nameof(inventoryNumber));
        if (dailyEnergyConsumptionKwh < 0)
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyConsumptionKwh), "Расход энергии не может быть отрицательным.");
        if (beginnerFriendlyScore is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nameof(beginnerFriendlyScore), "Оценка простоты должна быть от 1 до 10.");

        Name = name;
        InventoryNumber = inventoryNumber.Trim();
        DailyEnergyConsumptionKwh = dailyEnergyConsumptionKwh;
        BeginnerFriendlyScore = beginnerFriendlyScore;
        IsTechnicallySound = isTechnicallySound;
    }

    public string Name { get; }
    public string InventoryNumber { get; }
    public decimal DailyEnergyConsumptionKwh { get; }
    public int BeginnerFriendlyScore { get; }
    public bool IsTechnicallySound { get; }
}
