using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class GradesConfig : AuditableEntityConfig<Guid, Grade>
{
    public override void Configure(EntityTypeBuilder<Grade> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Grades)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.GradesM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
