using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class QuestionConfig : AuditableEntityConfig<Guid, Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Section)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.QuestionType)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.QuestionLevel)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.QuestionLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Grade)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.GradeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Text)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.TextId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SubjectId);
        builder.HasIndex(x => x.SectionId);
        builder.HasIndex(x => x.QuestionTypeId);
        builder.HasIndex(x => x.QuestionLevelId);
        builder.HasIndex(x => x.GradeId);
        builder.HasIndex(x => x.TextId);
        builder.HasIndex(x => x.AcademicYearId);


        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.QuestionsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
