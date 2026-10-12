namespace VyshKat.App.Domain.Contracts;

public interface ITransport : IInventoryItem, IEnergyConsumer
{
    int BeginnerFriendlyScore { get; }
    bool IsTechnicallySound { get; }
}
