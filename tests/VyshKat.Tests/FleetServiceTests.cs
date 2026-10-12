using Microsoft.Extensions.DependencyInjection;
using VyshKat.App.Domain.Contracts;
using VyshKat.App.Domain.Models;
using VyshKat.App.Infrastructure;

using VyshKat.App.Services;
using Xunit;

namespace VyshKat.Tests;

public sealed class FleetServiceTests
{
    [Fact]
    public void AcceptedTransport_IsAddedToFleet()
    {
        var service = CreateService(new FakeServiceCenter(true));

        var accepted = service.TryAddTransport(new Scooter("T-001", 1.5m, 7));

        Assert.True(accepted);
        Assert.Equal(1, service.TransportCount);
        Assert.Equal(1, service.TotalAssetCount);
    }

    [Fact]
    public void RejectedTransport_IsNotAddedToFleet()
    {
        var service = CreateService(new FakeServiceCenter(false));

        var accepted = service.TryAddTransport(new Scooter("T-002", 1.5m, 7));

        Assert.False(accepted);
        Assert.Equal(0, service.TransportCount);
        Assert.Equal(0, service.TotalAssetCount);
    }

    [Fact]
    public void InspectionDependency_IsResolvedFromDiContainer()
    {
        var service = CreateService(new FakeServiceCenter(false));

        Assert.False(service.TryAddTransport(new Scooter("T-003", 1m, 8)));
    }

    [Fact]
    public void BeginnerList_ContainsOnlyScoreSixOrMore()
    {
        var service = CreateService(new FakeServiceCenter(true));
        service.TryAddTransport(new Scooter("T-004", 1m, 6));
        service.TryAddTransport(new Scooter("T-005", 1m, 5));
        service.TryAddTransport(new Bicycle("T-006", 10));

        var result = service.GetBeginnerFriendlyTransport();

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.True(item.BeginnerFriendlyScore >= 6));
    }

    [Fact]
    public void EnergyReport_SumsAllEnergyConsumers()
    {
        var service = CreateService(new FakeServiceCenter(true));
        service.TryAddTransport(new Scooter("T-007", 1.8m, 7));
        service.AddInventoryItem(new ChargingStation("I-001", 3.2m));
        service.AddInventoryItem(new Helmet("I-002"));

        Assert.Equal(5.0m, service.TotalDailyEnergyConsumptionKwh);
    }

    [Fact]
    public void InventoryReport_IncludesTransportAndOtherAssets()
    {
        var service = CreateService(new FakeServiceCenter(true));
        service.TryAddTransport(new Scooter("T-008", 1m, 8));
        service.AddInventoryItem(new Helmet("I-003"));

        Assert.Equal(1, service.TransportCount);
        Assert.Equal(1, service.OtherAssetCount);
        Assert.Equal(2, service.TotalAssetCount);
        Assert.Contains(service.GetAllAssets(), item => item.InventoryNumber == "I-003");
    }

    [Fact]
    public void DuplicateInventoryNumber_IsRejected()
    {
        var service = CreateService(new FakeServiceCenter(true));
        service.AddInventoryItem(new Helmet("DUP-01"));

        Assert.Throws<InvalidOperationException>(() => service.AddInventoryItem(new Helmet("DUP-01")));
    }

    [Fact]
    public void TransportCannotBeAddedAsOrdinaryInventoryItem()
    {
        var service = CreateService(new FakeServiceCenter(true));

        Assert.Throws<ArgumentException>(() => service.AddInventoryItem(new Scooter("T-009", 1m, 7)));
    }

    [Fact]
    public void TransportWithFailedTechnicalInspection_IsRejectedByRealServiceCenter()
    {
        var service = CreateService(new ServiceCenter());

        var accepted = service.TryAddTransport(new Bicycle("T-010", 7, isTechnicallySound: false));

        Assert.False(accepted);
        Assert.Equal(0, service.TotalAssetCount);
    }

    private static IFleetService CreateService(IServiceCenter center)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IFleetInventory, FleetInventory>();
        services.AddSingleton<IServiceCenter>(center);
        services.AddSingleton<IFleetService, FleetService>();
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IFleetService>();
    }

    private sealed class FakeServiceCenter(bool result) : IServiceCenter
    {
        public bool Approve(ITransport transport) => result;
    }
}
