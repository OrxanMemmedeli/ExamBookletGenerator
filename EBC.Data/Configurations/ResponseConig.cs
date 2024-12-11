using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Configurations.Base;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class ResponseConig : AuditableEntityConfig<Guid, Response>
{
    public override void Configure(EntityTypeBuilder<Response> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.QuestionType)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SubjectId);
        builder.HasIndex(x => x.QuestionId);
        builder.HasIndex(x => x.QuestionTypeId);
        builder.HasIndex(x => x.AcademicYearId);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.ResponsesM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
