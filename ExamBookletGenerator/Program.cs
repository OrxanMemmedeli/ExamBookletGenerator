using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EBC.Business;
using EBC.Core;
using EBC.Core.Caching.Abstract;
using EBC.Core.Helpers.StartupFinders;
using EBC.Data;
using EBC.Data.Contexts;
using ExamBookletGenerator.Hubs;
using ExamBookletGenerator.Middlewares;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

ServiceOptions.Configure(
    useRateLimiting: false,
    useHangfire: false,
    useWatchDog: false,
    useHealthChecks: false,
    useMiniProfiler: false,
    useBackgroundService: false,
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

// ExtendedDbContextFactory-nı əlavə et
builder.Services.AddScoped<ExtendedDbContextFactory>();

// Middlewares
builder.Services.AddScoped<GlobalErrorHandlingMiddleware>(provider =>
{
    var serviceProvider = provider.GetRequiredService<IServiceProvider>();

    var cachingService = provider.GetRequiredService<ICachingService<IMemoryCache>>();

    return new GlobalErrorHandlingMiddleware(serviceProvider, cachingService, builder.Environment.IsDevelopment());
});


//Layers Services
builder.Services.AddCoreLayerServices(configuration: builder.Configuration);
builder.Services.AddBusinessLayerServices(configuration: builder.Configuration);
builder.Services.AddDataLayerServices();

builder.Services.AddSignalR();

var app = builder.Build();

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

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

// Authentication və Authorization istifadəsi
app.UseAuthentication(); // İstifadəçinin doğrulama vəziyyətini yoxlayır (login olub-olmadığını təsdiqləyir).
if (ServiceOptions.UseAuthenticationService)
    app.UseAuthorization();   // İstifadəçi icazələrini yoxlayır (resurslara çıxış icazələrini idarə edir).

//Layers App
await app.UseCoreLayerCustomApplication();
await app.UseBusinessLayerCustomApplication();
//SeedData melumati set edilir. 
await app.Services.AddSeedDataAsync();

//Hangfire
if (ServiceOptions.UseHangfire)
    app.MapHangfireDashboard(); ///hangfire

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapHub<UserActivityHub>("/userActivityHub");

app.Run();

