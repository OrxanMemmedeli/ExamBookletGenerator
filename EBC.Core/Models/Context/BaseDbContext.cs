using EBC.Core.Entities;
using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Identity;
using EBC.Core.Entities.Identity;
using EBC.Core.Helpers.Authentication;
using EBC.Core.Helpers.StartupFinders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EBC.Core.Models.Context;

/// <summary>
/// Layihənin əsas DbContext sinfi, yalnız `Core` layihəsinin `Entity`-lərini idarə edir.
/// Bu sinif verilənlər bazası ilə əlaqə qurmaq, `Entity` obyektlərini və onların konfiqurasiyalarını idarə etmək üçün istifadə olunur.
/// </summary>
public abstract class BaseDbContext : DbContext
{
    /// <summary>
    /// Parametrsiz konstruktor, əsas `DbContext` sinfinin miras alındığı sinfi yaradır.
    /// </summary>
    public BaseDbContext() { }

    /// <summary>
    /// Konfiqurasiya parametrləri ilə `DbContext`-i yaradır.
    /// </summary>
    /// <param name="options">DbContext üçün konfiqurasiya variantları.</param>
    public BaseDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// DbContext-in konfiqurasiyasını müəyyənləşdirir və `ConnectionString` istifadə edərək verilənlər bazasına qoşulmanı təmin edir.
    /// Bu metod əsasən `Migration` yaratma zamanı istifadə olunur.
    /// </summary>
    /// <param name="optionsBuilder">Verilənlər bazası konfiqurasiyası üçün istifadə olunur.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Application layihəsinin kök qovluğunu tapmaq
            var basePath = AppContext.BaseDirectory;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build(); //IConfigurationRoot obyektini yaradır. 

            /*
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true):

                "appsettings.json": Konfiqurasiya faylının adı (appsettings.json). Bu fayl konfiqurasiya məlumatlarını saxlayır.
                optional: false: Bu parametr faylın mövcud olub-olmamasını müəyyən edir. false olaraq təyin edildiyinə görə appsettings.json faylı mütləq mövcud olmalıdır. Əgər fayl mövcud deyilsə, səhv yaranacaq.
                reloadOnChange: true: Fayl dəyişdirildikdə konfiqurasiya məlumatlarının avtomatik olaraq yenilənməsini təmin edir. Fayl üzərində dəyişiklik edildikdə tətbiqə yenidən başlamağa ehtiyac olmadan yeni məlumatlar avtomatik olaraq oxunur.
             */

            string connectionString = ConnectionStringFinder.GetConnectionString(configuration);

            optionsBuilder.UseSqlServer(connectionString, option =>
            {
                option.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null); // connection zamani xeta alinarsa

                option.CommandTimeout(60); // Sorğunun maksimum icra müddətini 60 saniyə olaraq təyin edir
            });
        }
    }


    // Core layihəsindəki Entity-lər burada təyin edilir
    //public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<OrganizationAdress> OrganizationAdresses { get; set; }
    public DbSet<OrganizationAdressRole> OrganizationAdressRoles { get; set; }
    public DbSet<SysException> SysExceptions { get; set; }


    /// <summary>
    /// `OnModelCreating` metodu ilə bütün `Entity` konfiqurasiyaları tətbiq olunur.
    /// Cari layihədəki bütün `IEntityTypeConfiguration` interfeysindən miras alanları avtomatik olaraq konfiqurasiya edir.
    /// </summary>
    /// <param name="modelBuilder">Model yaratma vasitəsilə tətbiq olunan konfiqurasiya.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddConfigurations(modelBuilder);
    }

    private static void AddConfigurations(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new BaseEntityConfig());
        modelBuilder.ApplyConfiguration(new OrganizationAdressRoleConfig());
        modelBuilder.ApplyConfiguration(new UserRoleConfig());
    }

    /// <summary>
    /// Verilənlər bazasına dəyişiklikləri saxlamaq üçün metodu ləğv edir və əvvəlcə `OnBeforeSave` metodunu çağırır.
    /// </summary>
    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges(true);
    }

    /// <summary>
    /// Verilənlər bazasına dəyişiklikləri saxlamaq üçün metodu ləğv edir və əvvəlcə `OnBeforeSave` metodunu çağırır.
    /// `acceptAllChangesOnSuccess` parametri ilə dəyişikliklərin təsdiq olunmasını idarə edir.
    /// </summary>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    /// Verilənlər bazasına dəyişiklikləri asinxron olaraq saxlamaq üçün metodu ləğv edir və `OnBeforeSave` metodunu çağırır.
    /// `acceptAllChangesOnSuccess` parametri ilə dəyişikliklərin təsdiq olunmasını idarə edir.
    /// </summary>
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Verilənlər bazasına dəyişiklikləri asinxron olaraq saxlamaq üçün metodu ləğv edir və `OnBeforeSave` metodunu çağırır.
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(true, cancellationToken);
    }

    /// <summary>
    /// Yeni və dəyişdirilmiş `Entity`-lərə əlavə olunmadan əvvəl müəyyən sahələri təyin edən məntiq.
    /// Bu metod yeni `Entity`-lərə `CreatedDate`, dəyişdirilmiş `Entity`-lərə isə `ModifiedDate` təyin edir.
    /// </summary>
    private void OnBeforeSave()
    {
        var addedEntities = ChangeTracker.Entries()
            .Where(i => i.State == EntityState.Added && i.Entity is BaseEntity<Guid>)
            .Select(x => (BaseEntity<Guid>)x.Entity);

        var editedEntities = ChangeTracker.Entries()
            .Where(i => i.State == EntityState.Modified && i.Entity is BaseEntity<Guid>)
            .Select(x => (BaseEntity<Guid>)x.Entity);

        var currentUser = CurrentUser.UserId;

        PrepareAddedEntities(addedEntities, currentUser);
        PrepareEditedEntities(editedEntities, currentUser);
    }

    /// <summary>
    /// Yeni əlavə edilmiş `Entity`-lər üçün standart dəyərləri təyin edir.
    /// </summary>
    /// <param name="addedEntities">Yeni əlavə edilmiş `Entity`-lərin siyahısı.</param>
    private void PrepareAddedEntities(IEnumerable<BaseEntity<Guid>> addedEntities, Guid? currentUser)
    {
        foreach (var entity in addedEntities)
        {
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.Status = true;
            entity.IsDeleted = false;

            //User Ile inteqrasiyası olanlar üçün fieldlər set edilir.
            if (entity is IAuditable auditableEntity)
            {
                SetPropertyIfExists(entity, "CreatedUserId", currentUser);
                SetPropertyIfExists(entity, "ModifiedUserId", currentUser);

                // Lazım olarsa navigasiya sahələrini də təyin edə bilərsiniz:
                SetPropertyIfExists(entity, "CreatedUser", null);
                SetPropertyIfExists(entity, "ModifiedUser", null);
            }
        }
    }

    /// <summary>
    /// Dəyişdirilmiş `Entity`-lər üçün `ModifiedDate` sahəsini yeniləyir.
    /// </summary>
    /// <param name="editedEntities">Dəyişdirilmiş `Entity`-lərin siyahısı.</param>
    private void PrepareEditedEntities(IEnumerable<BaseEntity<Guid>> editedEntities, Guid? currentUser)
    {
        foreach (var entity in editedEntities)
        {
            entity.ModifiedDate = DateTime.Now;

            //User Ile inteqrasiyası olanlar üçün fieldlər set edilir.
            if (entity is IAuditable auditableEntity)
            {
                SetPropertyIfExists(entity, "ModifiedUserId", currentUser);
                SetPropertyIfExists(entity, "ModifiedUser", null);
            }
        }
    }

    private void SetPropertyIfExists(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
        }
    }
}
