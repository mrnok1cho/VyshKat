namespace VyshKat.App.Domain.Models;

public sealed class Bicycle : TransportBase
{
    public Bicycle(string inventoryNumber, int beginnerFriendlyScore, bool isTechnicallySound = true)
        : base("Велосипед", inventoryNumber, 0m, beginnerFriendlyScore, isTechnicallySound)
    {
    }
}
