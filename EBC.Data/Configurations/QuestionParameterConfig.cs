using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class QuestionParameterConfig : BaseEntityConfig<QuestionParameter, Guid>
{
    public override void Configure(EntityTypeBuilder<QuestionParameter> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.QuestionType)
            .WithMany(x => x.QuestionParameters)
            .HasForeignKey(x => x.QuestionTypeId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.SubjectParameter)
            .WithMany(x => x.QuestionParameters)
            .HasForeignKey(x => x.SubjectParameterId)
            .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.CreatUser)
        //    .WithMany(x => x.QuestionParameters)
        //    .HasForeignKey(x => x.CreatUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        //builder.HasOne(x => x.ModifyUser)
        //    .WithMany(x => x.QuestionParametersM)
        //    .HasForeignKey(x => x.ModifyUserId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}
