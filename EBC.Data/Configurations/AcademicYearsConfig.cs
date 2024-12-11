using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class AcademicYearsConfig : AuditableEntityConfig<Guid, AcademicYear>
{
    public override void Configure(EntityTypeBuilder<AcademicYear> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.AcademicYears)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.AcademicYearsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
