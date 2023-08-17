using MeuContexto.Context;

using MeuContexto;
using MeuContexto.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Transactions;
using Service;
using MeuContexto.Repositorys;

var builder = WebApplication.CreateBuilder(args);

string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<SeedUserRoleInitial>();
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); 

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
