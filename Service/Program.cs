using MeuContexto.Context;

using MeuContexto;
using MeuContexto.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Transactions;
using Service;
using MeuContexto.EntityRepositories;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<EntityContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<SeedUserRoleInitial>();
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");

var strings = builder.Configuration.GetConnectionString("DefaultConnections");

builder.Services.AddDbContext<EntityContext>(options => options.UseSqlServer(strings));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
