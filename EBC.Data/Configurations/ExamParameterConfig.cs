using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class ExamParameterConfig : BaseEntityConfig<ExamParameter, Guid>
{
    public override void Configure(EntityTypeBuilder<ExamParameter> builder)
    {
        base.Configure(builder);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.ExamParameters)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.ExamParametersM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
