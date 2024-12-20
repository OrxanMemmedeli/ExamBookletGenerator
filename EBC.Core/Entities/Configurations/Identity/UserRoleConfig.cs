﻿using EBC.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Core.Entities.Configurations.Identity;

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.UserId });
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
    }
}
