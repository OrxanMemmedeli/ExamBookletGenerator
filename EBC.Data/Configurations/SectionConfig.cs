using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class SectionConfig : AuditableEntityConfig<Guid, Section>
{
    public override void Configure(EntityTypeBuilder<Section> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SubjectId);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.SectionsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
