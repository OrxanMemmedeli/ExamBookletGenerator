using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class QuestionLevelConfig : BaseEntityConfig<QuestionLevel, Guid>
{
    public override void Configure(EntityTypeBuilder<QuestionLevel> builder)
    {
        base.Configure(builder);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.QuestionLevels)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.QuestionLevelsM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
