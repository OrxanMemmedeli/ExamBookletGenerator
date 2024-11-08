using AccountManagerSystem.Repositories.Abstract;
using AspNetCoreRateLimit;
using EBC.Core.Caching.Abstract;
using EBC.Core.Caching.Concrete;
using EBC.Core.Constants;
using EBC.Core.Helpers.StartupFinders;
using EBC.Core.Middlewares;
using EBC.Core.Models;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Core.Services.Abstract;
using EBC.Core.Services.BackgroundServices;
using EBC.Core.Services.Concrete;
using EBC.Core.Services.EmailService;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling;
using System.Reflection;
using WatchDog;
using WatchDog.src.Enums;

namespace EBC.Core;



public static class ServiceRegistration
{
    /// <summary>
    /// IOC container-ə core layer üçün implementasiyalarını qeyd edir.
    /// </summary>
    /// <param name="services">Service kolleksiyası.</param>
    /// <returns>Genişlənmiş service kolleksiyası.</returns>
    public static IServiceCollection AddCoreLayerServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        string connectionString = ConnectionStringFinder.GetConnectionString(configuration);

        // Common Services
        AddCoreServiceRouteServices(services, configuration);

        // Repositories
        AddRepositoryServices(services);

        // Middlewares
        AddMiddlewares(services, isDevelopment);

        // Health Checks
        RegisterHealthChecks(services, configuration);

        // MiniProfiler
        RegisterMiniProfiler(services);

        // WatchDog
        RegisterWatchDog(services, configuration);

        // SeedData
        services.AddScoped<EBC.Core.SeedData.SeedData>();

        // RateLimit
        RegisterRateLimit(services, configuration);

        // Hangfire
        RegisterHangfire(services, configuration);

        // BacgroundService
        RegisterBackgroundService(services, configuration);

        return services;
    }




    #region Common
    private static void AddCoreServiceRouteServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        //Core.Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddSingleton<ICachingService<IMemoryCache>, MemoryCachingService>();
        services.AddScoped<IEmailService, GmailService>();

        services.Configure<GoogleReCaptureConfigModel>(options =>
        {
            configuration.GetSection(GoogleReCaptureConfigModel.GoogleConfig).Bind(options);
        }); //recaptcha

    }

    private static void AddRepositoryServices(IServiceCollection services)
    {
        // Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepositoryWithoutBase<>), typeof(GenericRepository<>));

        // Xüsusi repository-ləri qeyd edin (əgər varsa)
        AddedRepoWithReflection(services);
        #region OldVersion
        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<IRoleRepository, RoleRepository>();
        //services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        //services.AddScoped<IOrganizationAdressRepository, OrganizationAdressRepository>();
        //services.AddScoped<IOrganizationAdressRoleRepository, OrganizationAdressRoleRepository>();
        #endregion

    }

    private static void AddedRepoWithReflection(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var repositoryTypes = assembly.GetTypes()
            .Where(type => type.IsClass
                && !type.IsAbstract
                && !type.IsNested
                && !type.IsGenericType
                && type.Name.EndsWith("Repository"))
            .ToList();

        var repositoryInterfaceTypes = assembly.GetTypes()
            .Where(type => type.IsInterface
                && !type.IsGenericType
                && type.Name.StartsWith("I")
                && type.Name.EndsWith("Repository"))
            .ToList();

        foreach (var repositoryType in repositoryTypes)
        {
            var repositoryInterfaceType = repositoryInterfaceTypes
                .SingleOrDefault(x => x.Name.Equals($"I{repositoryType.Name}", StringComparison.OrdinalIgnoreCase));

            if (repositoryInterfaceType != null)
                services.AddScoped(repositoryInterfaceType, repositoryType);
        }
    }
    private static void AddMiddlewares(IServiceCollection services, bool isDevelopment)
    {
        // Middleware qeydiyyatı
        services.AddScoped<GlobalErrorHandlingMiddleware>(provider =>
        {
            var serviceProvider = provider.GetRequiredService<IServiceProvider>();

            var cachingService = provider.GetRequiredService<ICachingService<IMemoryCache>>();

            return new GlobalErrorHandlingMiddleware(serviceProvider, cachingService, isDevelopment);
        });
    }
    #endregion



    #region Optional
    private static void RegisterHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseHealthChecks)
            return;

        services.AddHealthChecks()
            .AddSqlServer(ConnectionStringFinder.GetConnectionString(configuration), name: "SQL Database", tags: new[] { "db", "sql" });

        services.AddHealthChecksUI(setup =>
        {
            setup.AddHealthCheckEndpoint("Health Checks", "/health");
        }).AddInMemoryStorage();

    }

    private static void RegisterMiniProfiler(IServiceCollection services)
    {
        if (!ServiceOptions.UseMiniProfiler)
            return;

        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.ColorScheme = ColorScheme.Dark;
            options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
        }).AddEntityFramework();

    }

    /// <summary>
    /// Rate limiting xidmətlərini tətbiqə əlavə edir.
    /// </summary>
    /// <param name="services">DI konteksinə xidmətləri əlavə edən <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Konfiqurasiya məlumatları.</param>
    private static void RegisterRateLimit(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseRateLimiting)
            return;

        // `appsettings.json`-dan konfiqurasiya ayarlarını yükləyirik
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

        // Politika və verilərin yaddaşda saxlanmasını təmin edirik
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

        // ClientId və ClientIp məlumatlarının əldə edilməsi üçün HttpContextAccessor istifadə olunur
        services.AddHttpContextAccessor();

        // Əsasən `async` əməliyyatları idarə etmək üçün `AsyncKeyLockProcessingStrategy` əlavə olunur
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        // Rate limiting xidmətlərini idarə edən əsas konfiqurasiya xidməti əlavə olunur
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    private static void RegisterHangfire(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseHangfire)
            return;

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(ConnectionStringFinder.GetConnectionString(configuration));
        });
        services.AddHangfireServer();
    }

    private static void RegisterWatchDog(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseWatchDog)
            return;

        services.AddWatchDogServices(options =>
        {
            options.IsAutoClear = true;
            options.ClearTimeSchedule = WatchDog.src.Enums.WatchDogAutoClearScheduleEnum.Monthly;
            options.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
            options.SetExternalDbConnString = ConnectionStringFinder.GetConnectionString(configuration);
        });
    }

    private static void RegisterBackgroundService(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseWatchDog)
            return;

        services.AddSingleton<IBackgroundTaskQueue<string>>(provider => new BackgroundTaskQueue<string>(configuration));
        services.AddHostedService<QueueHostedService>();
    }
    #endregion

}
