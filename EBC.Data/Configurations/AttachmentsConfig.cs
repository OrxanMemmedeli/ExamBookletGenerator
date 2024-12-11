using EBC.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EBC.Data.Entities;

namespace EBC.Data.Configurations;

public class AttachmentsConfig : AuditableEntityConfig<Guid, Attachment>
{
    public override void Configure(EntityTypeBuilder<Attachment> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.AttachmentsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}