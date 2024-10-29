using EBC.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Entities.Configurations.Identity;

public class OrganizationAdressRoleConfig : IEntityTypeConfiguration<OrganizationAdressRole>
{
    public void Configure(EntityTypeBuilder<OrganizationAdressRole> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.OrganizationAdressId });
        builder.HasOne(x => x.OrganizationAdress).WithMany(x => x.OrganizationAdressRoles).HasForeignKey(x => x.OrganizationAdressId);
        builder.HasOne(x => x.Role).WithMany(x => x.OrganizationAdressRoles).HasForeignKey(x => x.RoleId);
    }
}
