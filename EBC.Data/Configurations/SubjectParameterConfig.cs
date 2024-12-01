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
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ExamParameter)
            .WithMany(x => x.SubjectParameters)
            .HasForeignKey(x => x.ExamParameterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SubjectId);
        builder.HasIndex(x => x.ExamParameterId);

    }
}
