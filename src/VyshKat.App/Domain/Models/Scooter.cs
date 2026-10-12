namespace VyshKat.App.Domain.Models;

public sealed class Scooter : TransportBase
{
    public Scooter(string inventoryNumber, decimal dailyEnergyConsumptionKwh, int beginnerFriendlyScore, bool isTechnicallySound = true)
        : base("Электросамокат", inventoryNumber, dailyEnergyConsumptionKwh, beginnerFriendlyScore, isTechnicallySound)
    {
    }
}
