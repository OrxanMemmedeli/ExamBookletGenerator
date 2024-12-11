using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Configurations;

public class SubjectsConfig : AuditableEntityConfig<Guid, Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Subjects)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.SubjectsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}