using MeuContexto;
using MeuContexto.Context;
using MeuContexto.Repositorys;
using MeuContexto.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<PersonTransaction>();
builder.Services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<SeedUserRoleInitial>();
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
