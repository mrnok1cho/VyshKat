namespace VyshKat.App.Domain.Contracts;

public interface IFleetService
{
    bool TryAddTransport(ITransport transport);
    void AddInventoryItem(IInventoryItem item);
    int TransportCount { get; }
    int OtherAssetCount { get; }
    int TotalAssetCount { get; }
    decimal TotalDailyEnergyConsumptionKwh { get; }
    IReadOnlyCollection<ITransport> GetBeginnerFriendlyTransport();
    IReadOnlyCollection<IInventoryItem> GetAllAssets();
}
