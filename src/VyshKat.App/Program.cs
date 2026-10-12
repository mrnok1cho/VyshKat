using Microsoft.Extensions.DependencyInjection;
using VyshKat.App.ConsoleUI;
using VyshKat.App.Infrastructure;
using VyshKat.App.Domain.Contracts;
using VyshKat.App.Services;

var services = new ServiceCollection();
services.AddSingleton<IFleetInventory, FleetInventory>();
services.AddSingleton<IServiceCenter, ServiceCenter>();
services.AddSingleton<IFleetService, FleetService>();
services.AddTransient<ConsoleMenu>();

using var provider = services.BuildServiceProvider();
provider.GetRequiredService<ConsoleMenu>().Run();

