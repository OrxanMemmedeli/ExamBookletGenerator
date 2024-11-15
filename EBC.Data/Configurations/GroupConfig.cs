using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class GroupConfig : BaseEntityConfig<Group, Guid>
{
    public override void Configure(EntityTypeBuilder<Group> builder)
    {
        base.Configure(builder);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.Groups)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.GroupsM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
