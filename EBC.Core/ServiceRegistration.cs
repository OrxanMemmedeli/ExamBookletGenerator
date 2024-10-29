using AccountManagerSystem.Repositories.Abstract;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Core.Services.Abstract;
using EBC.Core.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Core;

public static class ServiceRegistration
{
    /// <summary>
    /// IOC container-ə core layer üçün implementasiyalarını qeyd edir.
    /// </summary>
    /// <param name="services">Service kolleksiyası.</param>
    /// <returns>Genişlənmiş service kolleksiyası.</returns>
    public static IServiceCollection AddCoreLayerServices(this IServiceCollection services)
    {
        AddRepositoryServices(services);
        AddCoreServiceRouteServices(services);

        return services;
    }

    private static void AddCoreServiceRouteServices(IServiceCollection services)
    {
        //Core.Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUrlService, UrlService>();
    }

    private static void AddRepositoryServices(IServiceCollection services)
    {
        // Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepositoryWithoutBase<>), typeof(GenericRepository<>));

        // Xüsusi repository-ləri qeyd edin (əgər varsa)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IOrganizationAdressRepository, OrganizationAdressRepository>();
        services.AddScoped<IOrganizationAdressRoleRepository, OrganizationAdressRoleRepository>();
    }
}
