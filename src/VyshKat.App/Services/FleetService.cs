using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Services;

public sealed class FleetService : IFleetService
{
    private readonly IFleetInventory _inventory;
    private readonly IServiceCenter _serviceCenter;

    public FleetService(IFleetInventory inventory, IServiceCenter serviceCenter)
    {
        _inventory = inventory;
        _serviceCenter = serviceCenter;
    }

    public int TransportCount => _inventory.Assets.OfType<ITransport>().Count();
    public int OtherAssetCount => _inventory.Assets.Count - TransportCount;
    public int TotalAssetCount => _inventory.Assets.Count;

    public decimal TotalDailyEnergyConsumptionKwh =>
        _inventory.Assets.OfType<IEnergyConsumer>().Sum(asset => asset.DailyEnergyConsumptionKwh);

    public bool TryAddTransport(ITransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);

        if (_inventory.ContainsInventoryNumber(transport.InventoryNumber))
            throw new InvalidOperationException($"Инвентарный номер {transport.InventoryNumber} уже используется.");

        if (!_serviceCenter.Approve(transport))
            return false;

        _inventory.Add(transport);
        return true;
    }

    public void AddInventoryItem(IInventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item is ITransport)
            throw new ArgumentException("Транспорт добавляется только после техосмотра.", nameof(item));
        _inventory.Add(item);
    }

    public IReadOnlyCollection<ITransport> GetBeginnerFriendlyTransport() =>
        _inventory.Assets.OfType<ITransport>()
            .Where(transport => transport.BeginnerFriendlyScore >= 6)
            .ToArray();

    public IReadOnlyCollection<IInventoryItem> GetAllAssets() => _inventory.Assets.ToArray();
}
