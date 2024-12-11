using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class QuestionParameterConfig : AuditableEntityConfig<Guid, QuestionParameter>
{
    public override void Configure(EntityTypeBuilder<QuestionParameter> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.QuestionType)
            .WithMany(x => x.QuestionParameters)
            .HasForeignKey(x => x.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SubjectParameter)
            .WithMany(x => x.QuestionParameters)
            .HasForeignKey(x => x.SubjectParameterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.QuestionTypeId);
        builder.HasIndex(x => x.SubjectParameterId);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.QuestionParameters)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.QuestionParametersM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
