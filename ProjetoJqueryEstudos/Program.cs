using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using ProjetoJqueryEstudos.Context;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;
using ProjetoJqueryEstudos.Service;
using ProjetoJqueryEstudos.Transactions;
using ProjetoJqueryEstudos.UOW;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));



string connectionString = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<PersonTransaction>();
builder.Services.AddIdentity<UserIdentity, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<SeedUserRoleInitial>();
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews(options =>
{

});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


using (var scope = app.Services.CreateScope())
{
    var seedUserRoleInitial = scope.ServiceProvider.GetRequiredService<SeedUserRoleInitial>();
    seedUserRoleInitial.SeedRoles();
    seedUserRoleInitial.SeedUsers();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
