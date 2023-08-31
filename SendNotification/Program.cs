using MeuContexto.Context;
using MeuContexto.UOW;
using Microsoft.EntityFrameworkCore;
using SendNotification;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
