using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class SubjectParameterConfig : BaseEntityConfig<SubjectParameter, Guid>
{
    public override void Configure(EntityTypeBuilder<SubjectParameter> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.SubjectParameters)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.ExamParameter)
            .WithMany(x => x.SubjectParameters)
            .HasForeignKey(x => x.ExamParameterId)
            .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.SubjectParameters)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.SubjectParametersM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
