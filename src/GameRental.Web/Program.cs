using GameRental.Application;
using GameRental.Infrastructure;
using GameRental.Infrastructure.EF;
using GameRental.Infrastructure.Email;
using GameRental.Web.Extensions;
using GameRental.Web.Mapping;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


//Add serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console());


builder.Services.AddControllersWithViews();

//Add layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Environment.WebRootPath);

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapProfile));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var emailconfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);
    

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "Administration",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

//Migrate db and add seedData
await DatabaseExtensions.MigrateDatabase(app);

app.Run();