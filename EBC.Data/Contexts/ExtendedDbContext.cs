using EBC.Core.Entities;
using EBC.Core.Entities.Common;
using EBC.Core.Models.Context;
using EBC.Data.Entities;
using EBC.Data.Entities.CombineEntities;
using EBC.Data.Entities.ExceptionalEntities;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Contexts;

public class ExtendedDbContext : BaseDbContext
{
	public ExtendedDbContext() : base() {}
	public ExtendedDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }


    #region Base Entities With User
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<Booklet> Booklets { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamParameter> ExamParameters { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionLevel> QuestionLevels { get; set; }
    public DbSet<QuestionParameter> QuestionParameters { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectParameter> SubjectParameters { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    #endregion



    #region Simple Entities
    public DbSet<Payment> Payments { get; set; }
    #endregion



    #region Combination Entities
    public DbSet<QuestionAttahment> QuestionAttahments { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    #endregion



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

}
