using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class UserTypeConfig : BaseEntityConfig<UserType, Guid>
{
    public override void Configure(EntityTypeBuilder<UserType> builder)
    {
        base.Configure(builder);
    }
}
