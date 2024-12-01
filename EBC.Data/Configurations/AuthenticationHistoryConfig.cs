using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class AuthenticationHistoryConfig : BaseEntityConfig<AuthenticationHistory, Guid>
{
    public override void Configure(EntityTypeBuilder<AuthenticationHistory> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.AuthenticationHistories)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CompanyId);

    }
}
