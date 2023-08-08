using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoJqueryEstudos.Context;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.UOW;
using SendNotification;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
