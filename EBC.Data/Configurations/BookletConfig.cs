using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class BookletConfig : BaseEntityConfig<Booklet, Guid>
{
    public override void Configure(EntityTypeBuilder<Booklet> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Grade)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.GradeId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Variant)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.VariantId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Exam)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Booklets)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.Booklets)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.BookletsM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
