using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Configurations;

public class QuestionLevelsConfig : AuditableEntityConfig<Guid, QuestionLevel>
{
    public override void Configure(EntityTypeBuilder<QuestionLevel> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.QuestionLevels)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.QuestionLevelsM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
