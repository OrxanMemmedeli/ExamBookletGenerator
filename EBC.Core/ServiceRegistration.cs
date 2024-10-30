using AccountManagerSystem.Repositories.Abstract;
using EBC.Core.Caching.Abstract;
using EBC.Core.Caching.Concrete;
using EBC.Core.Middlewares;
using EBC.Core.Models;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Core.SeedData;
using EBC.Core.Services.Abstract;
using EBC.Core.Services.Concrete;
using EBC.Core.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

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
        //Common Services
        AddCoreServiceRouteServices(services, configuration);

        //Repositories
        AddRepositoryServices(services);

        //Middlewares
        AddMiddlewares(services, isDevelopment);

        //SeedData
        services.AddScoped<EBC.Core.SeedData.SeedData>();


        return services;
    }


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
}
