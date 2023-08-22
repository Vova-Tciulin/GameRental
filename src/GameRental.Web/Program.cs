using GameRental.Application;
using GameRental.Infrastructure;
using GameRental.Infrastructure.Email;
using GameRental.Web.Mapping;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure();
builder.Services.AddApplication(builder.Environment.WebRootPath);
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

app.Run();