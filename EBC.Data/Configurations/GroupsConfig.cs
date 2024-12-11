using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EBC.Data.Configurations.Base;

namespace EBC.Data.Configurations;

public class GroupsConfig : AuditableEntityConfig<Guid, Group>
{
    public override void Configure(EntityTypeBuilder<Group> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.GroupsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
