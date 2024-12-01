using EBC.Data.Entities.CombineEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.CombineConfigs;

public class CompanyUserConfig : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder.HasKey(x => new { x.CompanyId, x.AppUserId });
        builder.HasOne(x => x.Company)
            .WithMany(x => x.CompanyUsers)
            .HasForeignKey(x => x.CompanyId);

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.CompanyUsers)
            .HasForeignKey(x => x.AppUserId);

        builder.HasIndex(x => x.CompanyId);
        builder.HasIndex(x => x.AppUserId);
        builder.HasIndex(x => new { x.CompanyId, x.AppUserId });
    }
}
