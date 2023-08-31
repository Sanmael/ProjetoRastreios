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
        public static IServiceCollection AddInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnections");
            services.AddDbContext<EntityContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<EntityContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");
            services.AddScoped<IUserTransaction, UserTransaction>();
            services.AddScoped<PersonTransaction>();
            services.AddScoped<TrackingTransaction>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
