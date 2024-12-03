using AspNetCoreRateLimit;
using EBC.Core.Attributes.Authentication;
using EBC.Core.Caching.Abstract;
using EBC.Core.Caching.Concrete;
using EBC.Core.Constants;
using EBC.Core.Helpers.Authentication;
using EBC.Core.Helpers.StartupFinders;
using EBC.Core.Jobs.Common;
using EBC.Core.Jobs.Models;
using EBC.Core.Middlewares;
using EBC.Core.Models;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Core.Services.Abstract;
using EBC.Core.Services.BackgroundServices;
using EBC.Core.Services.Concrete;
using EBC.Core.Services.EmailService;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
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
    public static IServiceCollection AddCoreLayerServices(
        this IServiceCollection services, 
        IConfiguration configuration, 
        bool isDevelopment)
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

        // Authentication
        RegisterAuthenticationServices(services, configuration);

        // DefaultCors
        RegisterDefaultCors(services);

        return services;
    }




    #region Common
    private static void AddCoreServiceRouteServices(IServiceCollection services, IConfiguration configuration)
    {
        //json fayl üzərindən bir basa class modelinə cast edilməsi üçün.
        services.AddOptions();

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


        #region Session Manager Service ucun konfiqurasiya

        services.AddSingleton<UserSessionManagerService>();
        services.AddHttpContextAccessor();

        // `CurrentUser` konfiqurasiyası
        var serviceProvider = services.BuildServiceProvider();
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        var userSessionManager = serviceProvider.GetService<UserSessionManagerService>();
        CurrentUser.Configure(httpContextAccessor, userSessionManager);

        #endregion

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



        //Eger global olaraq AddHttpContextAccessor istifade edilmeyibse buraya elave edilmelidir. (ClientId və ClientIp məlumatlarının əldə edilməsi üçün HttpContextAccessor istifadə olunur)



        // Əsasən `async` əməliyyatları idarə etmək üçün `AsyncKeyLockProcessingStrategy` əlavə olunur
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        // Rate limiting xidmətlərini idarə edən əsas konfiqurasiya xidməti əlavə olunur
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    private static void RegisterHangfire(IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseHangfire)
            return;

        services.Configure<JobTriggersOptions>(configuration.GetSection("JobTriggers"));

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(ConnectionStringFinder.GetConnectionString(configuration));
        });
        services.AddHangfireServer();


        // IServiceProvider yaradılır
        var serviceProvider = services.BuildServiceProvider();

        // Sistemdəki işləri qeydiyyatdan keçirir
        JobScheduler.RegisterJobs(serviceProvider);
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
        if (!ServiceOptions.UseBackgroundService)
            return;

        services.AddSingleton<IBackgroundTaskQueue<string>>(provider => new BackgroundTaskQueue<string>(configuration));
        services.AddHostedService<QueueHostedService>();
    }

    /// <summary>
    /// Authentication və Authorization xidmətlərini layihəyə əlavə edir.
    /// Cookie-based autentifikasiya və xüsusi rollara əsaslanan icazələr üçün konfiqurasiya edilə bilər.
    /// </summary>
    /// <param name="services">Servis kolleksiyası.</param>
    /// <param name="configuration">Konfiqurasiya məlumatlarını ehtiva edən IConfiguration obyekti.</param>
    public static void RegisterAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (!ServiceOptions.UseAuthenticationService)
            return;

        // Cookie Autentifikasiyası
        // Bu bölmə, istifadəçinin sessiyasını idarə etmək və autentifikasiya cookie-lərini təyin etmək üçün nəzərdə tutulub.
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(ApplicationCommonField.ExpireTimeSpan); // Cookie-nin müddəti bitmə vaxtı (90 dəqiqə).
            options.LoginPath = "/Account/Login"; // İstifadəçi doğrulama tələb olunan yerə daxil olduqda yönləndiriləcək login səhifəsi.
            options.LogoutPath = "/Account/LogOut"; // Çıxış etdikdən sonra yönləndiriləcək logout səhifəsi.
            options.AccessDeniedPath = "/Account/Denied"; // İcazəsi olmayan istifadəçi roluna görə yönləndiriləcək səhifə.
            options.SlidingExpiration = true; // İstifadəçi aktiv olduqda cookie müddəti yenilənir (müddət tükənməsinin qarşısını alır).

            // Təhlükəsizlik Təkmilləşdirmələri
            options.Cookie.HttpOnly = true; // Cookie yalnız server tərəfindən oxuna bilər (XSS hücumlarına qarşı qoruma).
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Cookie yalnız HTTPS ilə göndəriləcək (istehsalat mühitində təhlükəsizliyi artırır).
        });

        // Authorization Xidməti
        // Bu bölmə, layihədəki müxtəlif resurslara çıxış icazəsini idarə etmək üçün nəzərdə tutulub.
        services.AddAuthorization(options =>
        {
            // "AdminPolicy" - Yalnız Admin rolunda olan istifadəçilərə icazə verir.
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));

            // "ManagerPolicy" - Yalnız Admin və ya Manager rolunda olan istifadəçilərə icazə verir.
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager", "Admin"));

            // "UserPolicy" - Admin, Manager və ya User rolunda olan istifadəçilərə icazə verir.
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Manager", "Admin"));
        });

        // MVC Konfiqurasiyası ilə Authorization Filtri
        services.AddMvc(config =>
        {
            // Ümumi authorization siyasəti: bütün istifadəçilər doğrulanmış olmalıdır.
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            config.Filters.Add(new AuthorizeFilter(policy)); // Doğrulanmış istifadəçi tələb edən ümumi filtr.
            config.Filters.Add(typeof(CustomRoleControlFilterAttribute)); // Xüsusi rol yoxlama filtrini əlavə edir.
        });
    }
    
    private static void RegisterDefaultCors(IServiceCollection services)
    {
        if (!ServiceOptions.UseDefaultCors)
            return;

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

    }

    #endregion

}
