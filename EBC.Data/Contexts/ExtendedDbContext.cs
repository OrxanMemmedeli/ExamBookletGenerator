using EBC.Core.Entities.Common;
using EBC.Core.Models.Context;
using EBC.Data.Configurations;
using EBC.Data.Configurations.CombineConfigs;
using EBC.Data.Entities;
using EBC.Data.Entities.CombineEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EBC.Data.Contexts;

public class ExtendedDbContext : BaseDbContext
{
    public ExtendedDbContext() : base() { }
    public ExtendedDbContext(DbContextOptions<ExtendedDbContext> options) : base(options) { }


    #region Base Entities With User
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
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


    #region Combination Entities
    public DbSet<QuestionAttahment> QuestionAttahments { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    #endregion


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddConfigurations(modelBuilder);

        var auditableEntities = typeof(BaseEntity<>).Assembly.GetTypes()
            .Where(t => typeof(IAuditable).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var entityType in auditableEntities)
        {
            modelBuilder.Entity(entityType)
                .Property(typeof(Guid?), "CreatedUserId")
                .IsRequired(false);

            modelBuilder.Entity(entityType)
                .Property(typeof(Guid?), "ModifiedUserId")
                .IsRequired(false);

            modelBuilder.Entity(entityType)
                .HasOne(typeof(AppUser), "CreatedUser")
                .WithMany()
                .HasForeignKey("CreatedUserId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity(entityType)
                .HasOne(typeof(AppUser), "ModifiedUser")
                .WithMany()
                .HasForeignKey("ModifiedUserId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }

    private static void AddConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyUserConfig());
        modelBuilder.ApplyConfiguration(new QuestionAttahmentConfig());
        modelBuilder.ApplyConfiguration(new AppUserConfig());
        modelBuilder.ApplyConfiguration(new AuthenticationHistoryConfig());
        modelBuilder.ApplyConfiguration(new BookletConfig());
        modelBuilder.ApplyConfiguration(new CompanyConfig());
        modelBuilder.ApplyConfiguration(new ExamConfig());
        modelBuilder.ApplyConfiguration(new PaymentOrDebtConfig());
        modelBuilder.ApplyConfiguration(new PaymentSummaryConfig());
        modelBuilder.ApplyConfiguration(new QuestionConfig());
        modelBuilder.ApplyConfiguration(new QuestionParameterConfig());
        modelBuilder.ApplyConfiguration(new ResponseConig());
        modelBuilder.ApplyConfiguration(new SectionConfig());
        modelBuilder.ApplyConfiguration(new SendingEmailConfig());
        modelBuilder.ApplyConfiguration(new SubjectParameterConfig());
    }
}
