using System.Globalization;
using VyshKat.App.Domain.Contracts;
using VyshKat.App.Domain.Models;

namespace VyshKat.App.ConsoleUI;

public sealed class ConsoleMenu
{
    private readonly IFleetService _fleetService;

    public ConsoleMenu(IFleetService fleetService) => _fleetService = fleetService;

    public void Run()
    {
        SeedDemoData();
        Console.WriteLine("=== ВышКат: демонстрация стартовых сценариев ===");
        ShowReport();
        ShowBeginnerFriendlyTransport();
        ShowAllAssets();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Меню ===");
            Console.WriteLine("1. Добавить транспортное средство");
            Console.WriteLine("2. Добавить вещь на баланс");
            Console.WriteLine("3. Отчёт по парку и энергопотреблению");
            Console.WriteLine("4. Список техники для новичков");
            Console.WriteLine("5. Весь инвентарь с номерами");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine()?.Trim();
            try
            {
                switch (choice)
                {
                    case "1": AddTransport(); break;
                    case "2": AddInventoryItem(); break;
                    case "3": ShowReport(); break;
                    case "4": ShowBeginnerFriendlyTransport(); break;
                    case "5": ShowAllAssets(); break;
                    case "0": return;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Не удалось выполнить операцию: {ex.Message}");
            }
        }
    }

    private void SeedDemoData()
    {
        AddTransportToFleet(new Scooter("VK-001", 1.8m, 8, true));
        AddTransportToFleet(new Scooter("VK-002", 2.1m, 4, true));
        AddTransportToFleet(new ElectricBicycle("VK-003", 2.6m, 7, true));
        AddTransportToFleet(new Bicycle("VK-004", 9, false));
        _fleetService.AddInventoryItem(new Helmet("VH-001"));
        _fleetService.AddInventoryItem(new ChargingStation("VC-001", 3.2m));
        _fleetService.AddInventoryItem(new ChargingCabinet("VC-002", 4.8m));
    }

    private void AddTransport()
    {
        Console.WriteLine("Вид транспорта: 1 — электросамокат, 2 — велосипед, 3 — электровелосипед");
        var type = ReadRequired("Тип: ");
        var inventoryNumber = ReadRequired("Инвентарный номер: ");
        var score = ReadInt("Простота для новичка (1–10): ", 1, 10);
        var technicallySound = ReadInt("Техосмотр пройден? (1 — да, 0 — нет): ", 0, 1) == 1;

        ITransport transport = type switch
        {
            "1" => new Scooter(inventoryNumber, ReadDecimal("Суточный расход энергии (кВт·ч): "), score, technicallySound),
            "2" => new Bicycle(inventoryNumber, score, technicallySound),
            "3" => new ElectricBicycle(inventoryNumber, ReadDecimal("Суточный расход энергии (кВт·ч): "), score, technicallySound),
            _ => throw new ArgumentException("Выберите тип 1, 2 или 3.")
        };

        if (AddTransportToFleet(transport))
            Console.WriteLine("Транспорт прошёл техосмотр и принят в парк.");
        else
            Console.WriteLine("Транспорт отклонён: техосмотр не пройден.");
    }

    private bool AddTransportToFleet(ITransport transport) => _fleetService.TryAddTransport(transport);

    private void AddInventoryItem()
    {
        Console.WriteLine("Тип вещи: 1 — шлем, 2 — зарядная станция, 3 — зарядный шкаф");
        var type = ReadRequired("Тип: ");
        var inventoryNumber = ReadRequired("Инвентарный номер: ");

        IInventoryItem item = type switch
        {
            "1" => new Helmet(inventoryNumber),
            "2" => new ChargingStation(inventoryNumber, ReadDecimal("Суточный расход энергии (кВт·ч): ")),
            "3" => new ChargingCabinet(inventoryNumber, ReadDecimal("Суточный расход энергии (кВт·ч): ")),
            _ => throw new ArgumentException("Выберите тип 1, 2 или 3.")
        };

        _fleetService.AddInventoryItem(item);
        Console.WriteLine("Вещь добавлена на баланс.");
    }

    private void ShowReport()
    {
        Console.WriteLine("--- Отчёт по парку ---");
        Console.WriteLine($"Транспортных средств: {_fleetService.TransportCount}");
        Console.WriteLine($"Прочих вещей на балансе: {_fleetService.OtherAssetCount}");
        Console.WriteLine($"Всего объектов на балансе: {_fleetService.TotalAssetCount}");
        Console.WriteLine($"Суммарный суточный расход энергии: {_fleetService.TotalDailyEnergyConsumptionKwh:0.##} кВт·ч");
    }

    private void ShowBeginnerFriendlyTransport()
    {
        Console.WriteLine("--- Техника для новичков (простота ≥ 6) ---");
        var transport = _fleetService.GetBeginnerFriendlyTransport();
        if (transport.Count == 0)
        {
            Console.WriteLine("Подходящей техники нет.");
            return;
        }

        foreach (var item in transport)
            Console.WriteLine($"{item.Name} | {item.InventoryNumber} | простота: {item.BeginnerFriendlyScore}/10");
    }

    private void ShowAllAssets()
    {
        Console.WriteLine("--- Весь инвентарь ---");
        foreach (var asset in _fleetService.GetAllAssets().OrderBy(asset => asset.InventoryNumber))
        {
            var energy = asset is IEnergyConsumer consumer
                ? $" | расход: {consumer.DailyEnergyConsumptionKwh:0.##} кВт·ч/сутки"
                : string.Empty;
            Console.WriteLine($"{asset.Name} | инв. № {asset.InventoryNumber}{energy}");
        }
    }

    private static string ReadRequired(string prompt)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Поле не может быть пустым.");
        return value;
    }

    private static int ReadInt(string prompt, int min, int max)
    {
        Console.Write(prompt);
        if (!int.TryParse(Console.ReadLine(), out var value) || value < min || value > max)
            throw new ArgumentException($"Введите целое число от {min} до {max}.");
        return value;
    }

    private static decimal ReadDecimal(string prompt)
    {
        Console.Write(prompt);
        var raw = Console.ReadLine()?.Trim().Replace(',', '.');
        if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) || value < 0)
            throw new ArgumentException("Введите неотрицательное число (например, 1.8).");
        return value;
    }
}
