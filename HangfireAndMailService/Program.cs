
using Hangfire;
using HangfireAndMailService.Models;
using HangfireAndMailService.Services;
using HangfireAndMailService.Services.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HangContext>(x=>x.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, AppRole>(x=> x.Password = new PasswordOptions()
{
     RequireNonAlphanumeric = false,
      RequiredLength = 1,
       RequireLowercase = false,
        RequireUppercase = false
}).AddEntityFrameworkStores<HangContext>();




builder.Services.AddHangfire(
    config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer(x =>
{
    x.WorkerCount = 10; // �� say�s�
    x.StopTimeout = TimeSpan.FromMilliseconds(5); // Durdurma zaman a��m�
    x.ShutdownTimeout = TimeSpan.FromMinutes(1); // Kapatma zaman a��m� => Tekrar ba�latma s�resi
    x.ServerName = "Hangfire Server Yonetimi"; // Server ad�
    x.ServerCheckInterval = TimeSpan.FromMinutes(1); // Sunucu kontrol aral���
});

builder.Services.AddScoped<IMailService, MailManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseHangfireDashboard();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



var serviceProvider  = app.Services.CreateScope().ServiceProvider;
var mailService = serviceProvider.GetRequiredService<IMailService>();
RecurringJob.AddOrUpdate("BirthdayMail", () => mailService.BirthdayMail(), Cron.Minutely);

app.Run();



