using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EBC.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services)
    {

        // Repositories
        AddRepositoryServices(services);


        // SeedData
        services.AddScoped<EBC.Data.SeedData.SeedData>();

        return services;
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
}
