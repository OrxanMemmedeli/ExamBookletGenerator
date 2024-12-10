using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EBC.Data.Entities.Identity;

namespace EBC.Data.Configurations.Identity;

public class OrganizationAdressRoleConfig : IEntityTypeConfiguration<OrganizationAdressRole>
{
    public void Configure(EntityTypeBuilder<OrganizationAdressRole> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.OrganizationAdressId });
        builder.HasOne(x => x.OrganizationAdress).WithMany(x => x.OrganizationAdressRoles).HasForeignKey(x => x.OrganizationAdressId);
        builder.HasOne(x => x.Role).WithMany(x => x.OrganizationAdressRoles).HasForeignKey(x => x.RoleId);
    }
}
