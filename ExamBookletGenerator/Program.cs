using EBC.Core;
using EBC.Core.Helpers.StartupFinders;
using EBC.Data.Contexts;
using ExamBookletGenerator.Hubs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

ServiceOptions.Configure(
    useRateLimiting: true,
    useHangfire: true,
    useWatchDog: true,
    useHealthChecks: true,
    useMiniProfiler: true,
    useBackgroundService: true,
    useAuthenticationService: false,
    useDefaultCors: false
);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ExtendedDbContext>();
builder.Services.AddDbContext<DbContext, ExtendedDbContext>(conf =>
{
    string connectionString = ConnectionStringFinder.GetConnectionString(builder.Configuration);

    conf.UseSqlServer(connectionString, option =>
    {
        option.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null); // connection zamani xeta alinarsa

        option.CommandTimeout(60); // Sorğunun maksimum icra müddətini 60 saniyə olaraq təyin edir

    });

});


//Layers Services
builder.Services.AddCoreLayerServices(configuration: builder.Configuration, isDevelopment: builder.Environment.IsDevelopment());

builder.Services.AddSignalR();

var app = builder.Build();

//SeedData melumati set edilir. 
await app.Services.AddSeedDataAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// HTTPS yönləndirmə və statik faylların xidmətə qoşulması
app.UseHttpsRedirection();
app.UseStaticFiles();

// Sorğuların marşrutlaşdırılması
app.UseRouting();

// Authentication və Authorization istifadəsi
app.UseAuthentication(); // İstifadəçinin doğrulama vəziyyətini yoxlayır (login olub-olmadığını təsdiqləyir).
if (ServiceOptions.UseAuthenticationService)
    app.UseAuthorization();   // İstifadəçi icazələrini yoxlayır (resurslara çıxış icazələrini idarə edir).

//Layers App
await app.UseCoreLayerCustomApplication();

//Hangfire
if (ServiceOptions.UseHangfire)
    app.MapHangfireDashboard(); ///hangfire

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<UserActivityHub>("/userActivityHub");

app.Run();
