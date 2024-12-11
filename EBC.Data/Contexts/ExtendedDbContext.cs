using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Common;
using EBC.Core.Helpers.Authentication;
using EBC.Core.Helpers.StartupFinders;
using EBC.Data.Configurations;
using EBC.Data.Configurations.Base;
using EBC.Data.Configurations.CombineConfigs;
using EBC.Data.Configurations.Identity;
using EBC.Data.Entities;
using EBC.Data.Entities.CombineEntities;
using EBC.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EBC.Data.Contexts;
/// <summary>
/// Layihənin əsas DbContext sinfi, yalnız `Core` layihəsinin `Entity`-lərini idarə edir.
/// Bu sinif verilənlər bazası ilə əlaqə qurmaq, `Entity` obyektlərini və onların konfiqurasiyalarını idarə etmək üçün istifadə olunur.
/// </summary>
public class ExtendedDbContext : DbContext
{
    /// <summary>
    /// Parametrsiz konstruktor, əsas `DbContext` sinfinin miras alındığı sinfi yaradır.
    /// </summary>
    public ExtendedDbContext() : base() { }

    /// <summary>
    /// Konfiqurasiya parametrləri ilə `DbContext`-i yaradır.
    /// </summary>
    /// <param name="options">DbContext üçün konfiqurasiya variantları.</param>
    public ExtendedDbContext(DbContextOptions<ExtendedDbContext> options) : base(options) { }


    #region Base Entities With User
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<AuthenticationHistory> AuthenticationHistories { get; set; }
    public DbSet<Booklet> Booklets { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamParameter> ExamParameters { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<PaymentOrDebt> PaymentOrDebts { get; set; }
    public DbSet<PaymentSummary> PaymentSummaries { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionLevel> QuestionLevels { get; set; }
    public DbSet<QuestionParameter> QuestionParameters { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<SendingEmail> SendingEmails { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectParameter> SubjectParameters { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<UserType> UserTypes { get; set; }
    public DbSet<Variant> Variants { get; set; }

    #endregion

    public DbSet<Role> Roles { get; set; }
    public DbSet<OrganizationAdress> OrganizationAdresses { get; set; }
    public DbSet<SysException> SysExceptions { get; set; }


    #region Combination Entities
    public DbSet<QuestionAttahment> QuestionAttahments { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<OrganizationAdressRole> OrganizationAdressRoles { get; set; }



    #endregion


    /// <summary>
    /// DbContext-in konfiqurasiyasını müəyyənləşdirir və `ConnectionString` istifadə edərək verilənlər bazasına qoşulmanı təmin edir.
    /// Bu metod əsasən `Migration` yaratma zamanı istifadə olunur.
    /// </summary>
    /// <param name="optionsBuilder">Verilənlər bazası konfiqurasiyası üçün istifadə olunur.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            // Application layihəsinin kök qovluğunu tapmaq
            var basePath = Path.Combine(AppContext.BaseDirectory);

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

            optionsBuilder.UseSqlServer(connectionString);

            //optionsBuilder.UseSqlServer(connectionString, option =>
            //{
            //    option.EnableRetryOnFailure(
            //                maxRetryCount: 5,
            //                maxRetryDelay: TimeSpan.FromSeconds(30),
            //                errorNumbersToAdd: null); // connection zamani xeta alinarsa

            //    option.CommandTimeout(60); // Sorğunun maksimum icra müddətini 60 saniyə olaraq təyin edir
            //});
        }
    }

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
            .Where(i => i.State == EntityState.Added && i.Entity is BaseEntity<Guid> || i.Entity is AuditableEntity<Guid, User>)
            .Select(x => x.Entity);

        var editedEntities = ChangeTracker.Entries()
            .Where(i => i.State == EntityState.Modified && i.Entity is BaseEntity<Guid> || i.Entity is AuditableEntity<Guid, User>)
            .Select(x => x.Entity);

        var currentUser = CurrentUser.UserId;

        PrepareAddedEntities(addedEntities, currentUser);
        PrepareEditedEntities(editedEntities, currentUser);
    }

    /// <summary>
    /// Yeni əlavə edilmiş `Entity`-lər üçün standart dəyərləri təyin edir.
    /// </summary>
    /// <param name="addedEntities">Yeni əlavə edilmiş `Entity`-lərin siyahısı.</param>
    /// <param name="currentUser">Cari istifadəçi ID-si.</param>
    private void PrepareAddedEntities(IEnumerable<object> addedEntities, Guid? currentUser)
    {
        foreach (var entity in addedEntities)
        {
            if (entity is AuditableEntity<Guid, User> auditableEntity)
            {
                auditableEntity.CreatedDate = DateTime.Now;
                auditableEntity.ModifiedDate = DateTime.Now;
                auditableEntity.IsDeleted = false;
                auditableEntity.Status = true;
                auditableEntity.CreateUserId = currentUser ?? Guid.Empty; // Null yoxlaması
                auditableEntity.ModifyUserId = currentUser ?? Guid.Empty; // Null yoxlaması
            }
            else if (entity is BaseEntity<Guid> baseEntity)
            {
                baseEntity.CreatedDate = DateTime.Now;
                baseEntity.ModifiedDate = DateTime.Now;
                baseEntity.IsDeleted = false;
                baseEntity.Status = true;
            }
        }
    }


    /// <summary>
    /// Dəyişdirilmiş `Entity`-lər üçün `ModifiedDate` sahəsini yeniləyir.
    /// </summary>
    /// <param name="editedEntities">Dəyişdirilmiş `Entity`-lərin siyahısı.</param>
    /// <param name="currentUser">Cari istifadəçi ID-si.</param>
    private void PrepareEditedEntities(IEnumerable<object> editedEntities, Guid? currentUser)
    {
        foreach (var entity in editedEntities)
        {
            if (entity is AuditableEntity<Guid, User> auditableEntity)
            {
                auditableEntity.ModifiedDate = DateTime.Now;
                auditableEntity.ModifyUserId = currentUser ?? Guid.Empty; // Null yoxlaması
                auditableEntity.ModifiedDate = DateTime.Now;
            }
            else if (entity is BaseEntity<Guid> baseEntity)
            {
                baseEntity.ModifiedDate = DateTime.Now;
            }
        };
    }

}
