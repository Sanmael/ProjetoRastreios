using MeuContexto;
using MeuContexto.Context;
using MeuContexto.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Transactions;

namespace DependenceInjection
{
    public static class Dependences
    {
        public static IServiceCollection AddInfrastructure(IServiceCollection services)
        {

            string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");
            services.AddScoped<IUserTransaction, UserTransaction>();
            services.AddScoped<PersonTransaction>();
            services.AddScoped<TrackingTransaction>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
        public static IServiceCollection AddInfrastructureMicroServices(IServiceCollection services,string connectionString)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
