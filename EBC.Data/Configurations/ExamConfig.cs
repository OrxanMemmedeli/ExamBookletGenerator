using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class ExamConfig : BaseEntityConfig<Exam, Guid>
{
    public override void Configure(EntityTypeBuilder<Exam> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Grade)
            .WithMany(x => x.Exams)
            .HasForeignKey(x => x.GradeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ExamParameter)
            .WithMany(x => x.Exams)
            .HasForeignKey(x => x.ExamParameterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.GradeId);
        builder.HasIndex(x => x.ExamParameterId);


    }
}
