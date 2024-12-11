using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class SubjectParameterConfig : AuditableEntityConfig<Guid, SubjectParameter>
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


        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.SubjectParameters)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.SubjectParametersM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
