using EBC.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EBC.Data.Entities;

namespace EBC.Data.Configurations;

public class QuestionTypesConfig : AuditableEntityConfig<Guid, QuestionType>
{
    public override void Configure(EntityTypeBuilder<QuestionType> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreateUser)
            .WithMany(x => x.QuestionTypes)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifyUser)
            .WithMany(x => x.QuestionTypesM)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}