namespace VyshKat.App.Domain.Models;

public sealed class ElectricBicycle : TransportBase
{
    public ElectricBicycle(string inventoryNumber, decimal dailyEnergyConsumptionKwh, int beginnerFriendlyScore, bool isTechnicallySound = true)
        : base("Электровелосипед", inventoryNumber, dailyEnergyConsumptionKwh, beginnerFriendlyScore, isTechnicallySound)
    {
    }
}
