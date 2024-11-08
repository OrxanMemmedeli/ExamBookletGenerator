using EBC.Core;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

ServiceOptions.Configure(
    useRateLimiting: true,
    useHangfire: true,
    useWatchDog: true,
    useHealthChecks: true,
    useMiniProfiler: true,
    useBackgroundService: true
);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Layers Services
builder.Services.AddCoreLayerServices(configuration: builder.Configuration, isDevelopment: builder.Environment.IsDevelopment());


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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Layers App
app.UseCoreLayerCustomApplication();

//Hangfire
app.MapHangfireDashboard(); ///hangfire

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
