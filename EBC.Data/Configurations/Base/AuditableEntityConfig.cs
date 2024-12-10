using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.Base;


public class AuditableEntityConfig<Tid, T> : BaseEntityConfig<T, Tid> where T : AuditableEntity<Tid, User>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.CreateUserId);
        builder.HasIndex(x => x.ModifyUserId);
    }
}
