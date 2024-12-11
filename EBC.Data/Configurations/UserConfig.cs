using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace EBC.Data.Configurations;

public class UserConfig : BaseEntityConfig<User, Guid>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.UserType)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.UserTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserTypeId);

    }

    private void ConfigureFieldForCreate<T>(
        EntityTypeBuilder<User> builder,
        Expression<Func<User, IEnumerable<T>>> hasMany,
        Expression<Func<T, User>> withOne,
        Expression<Func<T, object>> hasForeignKey) where T : Entity<Guid>
    {
        builder.HasMany(hasMany)
            .WithOne(withOne)
            .HasForeignKey(hasForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
