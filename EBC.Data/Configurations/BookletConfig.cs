using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class BookletConfig : AuditableEntityConfig<Guid, Booklet>
{
    public override void Configure(EntityTypeBuilder<Booklet> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Grade)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.GradeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Variant)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.VariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Exam)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.GradeId);
        builder.HasIndex(x => x.GroupId);
        builder.HasIndex(x => x.VariantId);
        builder.HasIndex(x => x.ExamId);
        builder.HasIndex(x => x.AcademicYearId);


        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.BookletsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
