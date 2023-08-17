using MeuContexto.Context;
using MeuContexto.UOW;
using Microsoft.EntityFrameworkCore;
using SendTrackingMail;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<Worker>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
