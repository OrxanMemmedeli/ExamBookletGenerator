using EBC.Data.Entities.Identity;
using EBC.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EBC.Business.Helpers.StartupFinders;

/// <summary>
/// Sistemdə mövcud olan bütün kontroller və action-ları müəyyən edərək onların URL-lərini 
/// tapmaq və bu məlumatları <see cref="IOrganizationAdressRepository"/> ilə sinxronizasiya etmək üçün 
/// istifadə olunan köməkçi sinif.
/// </summary>
public static class OrganizationAddressFinder
{
    /// <summary>
    /// Sistemdəki kontroller və action-ları taparaq <see cref="IOrganizationAdressRepository"/> 
    /// üzərində yeni URL-ləri əlavə edən və artıq olmayanları silən asinxron metod.
    /// Bu versiya <see cref="IApplicationBuilder"/> ilə istifadə üçündür.
    /// </summary>
    /// <param name="app">Tətbiqin IApplicationBuilder instansı.</param>
    public static async Task GenerateAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var actionDescriptorCollectionProvider = scope.ServiceProvider.GetRequiredService<IActionDescriptorCollectionProvider>();
        var organizationRepo = scope.ServiceProvider.GetRequiredService<IOrganizationAdressRepository>();

        await GenerateOrganizationAddressesAsync(actionDescriptorCollectionProvider, organizationRepo);
    }


    /// <summary>
    /// Kontroller və action-ları taparaq <see cref="IOrganizationAdressRepository"/> 
    /// üzərində URL məlumatlarını sinxronizasiya edən asinxron metod.
    /// Bu versiya <see cref="IServiceCollection"/> ilə istifadə üçündür.
    /// </summary>
    /// <param name="services">Tətbiq servislərinin kolleksiyası.</param>
    public static async Task GenerateAsync(this IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var actionDescriptorCollectionProvider = scope.ServiceProvider.GetRequiredService<IActionDescriptorCollectionProvider>();
        var organizationRepo = scope.ServiceProvider.GetRequiredService<IOrganizationAdressRepository>();

        await GenerateOrganizationAddressesAsync(actionDescriptorCollectionProvider, organizationRepo);
    }


    /// <summary>
    /// Kontrollerlərin URL-lərini alaraq <see cref="IOrganizationAdressRepository"/> 
    /// üzərində yeni URL-ləri qeyd edən və artıq olmayan URL-ləri silən əsas metod.
    /// </summary>
    /// <param name="actionDescriptorProvider">Sistemdə mövcud olan kontroller və action-ların deskriptorları.</param>
    /// <param name="organizationRepo">Sinxronizasiya ediləcək <see cref="IOrganizationAdressRepository"/> instansı.</param>
    private static async Task GenerateOrganizationAddressesAsync(
        IActionDescriptorCollectionProvider actionDescriptorProvider,
        IOrganizationAdressRepository organizationRepo)
    {
        // Kontroller və action URL-lərini əldə edir
        var urlList = GetControllerActionUrls(actionDescriptorProvider.ActionDescriptors.Items);

        // Mövcud URL-ləri əldə edir
        var existingAddresses = (await organizationRepo.GetAll(false)).Select(x => x.RequestAdress).ToList();

        // Yaratma və silmə üçün lazımi URL-ləri müəyyən edir
        var createList = urlList.Except(existingAddresses).Select(url => new OrganizationAdress { RequestAdress = url }).ToList();
        var deleteList = existingAddresses.Except(urlList).Select(url => new OrganizationAdress { RequestAdress = url }).ToList();

        // Yaratma üçün qeydiyyatları əlavə edir
        if (createList.Any())
            organizationRepo.AddRangeWithoutSave(createList);

        // Silinməsi lazım olan qeydiyyatları əlavə edir
        if (deleteList.Any())
            organizationRepo.DeleteRangeWithoutSave(deleteList);

        await organizationRepo.SaveChangesAsync();
    }


    /// <summary>
    /// Kontroller və action URL-lərini müəyyən edən metod.
    /// </summary>
    /// <param name="actionDescriptors">Mövcud action deskriptorlarının siyahısı.</param>
    /// <returns>Bütün kontroller və action-ların URL siyahısı.</returns>
    private static List<string> GetControllerActionUrls(IReadOnlyList<ActionDescriptor> actionDescriptors)
    {
        var urlList = new List<string>();
        var controllerDescriptors = actionDescriptors.OfType<ControllerActionDescriptor>().ToList();

        // Area olan controller və action-ları əlavə edir
        var areaNames = GetDistinctAreaNames(controllerDescriptors);
        foreach (var area in areaNames)
            AddUrlsForAreaControllers(controllerDescriptors, area, urlList);

        // Area olmayan controller və action-ları əlavə edir
        AddUrlsForAreaControllers(controllerDescriptors, null, urlList);

        return urlList;
    }


    /// <summary>
    /// Sistemdə mövcud olan təkrarlanmayan Area adlarını əldə edən metod.
    /// </summary>
    /// <param name="controllerDescriptors">Kontroller deskriptorlarının siyahısı.</param>
    /// <returns>Təkrarlanmayan Area adlarının siyahısı.</returns>
    private static IEnumerable<string?> GetDistinctAreaNames(IEnumerable<ControllerActionDescriptor> controllerDescriptors)
    {
        return controllerDescriptors
            .Select(descriptor => descriptor.RouteValues.TryGetValue("area", out var areaValue) ? areaValue : null)
            .Where(area => area != null)
            .Distinct();
    }


    /// <summary>
    /// Müəyyən bir Area adı üçün kontroller və action URL-lərini <paramref name="urlList"/>-ə əlavə edən metod.
    /// </summary>
    /// <param name="controllerDescriptors">Kontroller deskriptorlarının siyahısı.</param>
    /// <param name="area">İşlənəcək Area adı. `null` dəyər area olmayan kontrollerlər üçün istifadə edilir.</param>
    /// <param name="urlList">URL-lərin əlavə ediləcəyi siyahı.</param>
    private static void AddUrlsForAreaControllers(IEnumerable<ControllerActionDescriptor> controllerDescriptors, string? area, List<string> urlList)
    {
        // Verilən Area üçün kontrollerlərin adlarını alır
        var controllersInArea = controllerDescriptors
            .Where(descriptor => string.Equals(descriptor.RouteValues["area"], area))
            .Select(descriptor => descriptor.ControllerName)
            .Distinct();

        // Kontrollerlərə uyğun action URL-lərini əlavə edir
        foreach (var controller in controllersInArea)
        {
            var actionsInController = controllerDescriptors
                .Where(descriptor => string.Equals(descriptor.RouteValues["area"], area) &&
                                     string.Equals(descriptor.ControllerName, controller))
                .Select(descriptor => descriptor.ActionName)
                .Distinct();

            foreach (var action in actionsInController)
            {
                var url = !string.IsNullOrEmpty(area)
                    ? $"/{area}/{controller}/{action}"
                    : $"/{controller}/{action}";

                urlList.Add(url);
            }
        }
    }
}
