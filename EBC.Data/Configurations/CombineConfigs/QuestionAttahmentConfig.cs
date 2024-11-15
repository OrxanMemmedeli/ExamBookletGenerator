using EBC.Data.Entities.CombineEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.CombineConfigs;

public class QuestionAttahmentConfig : IEntityTypeConfiguration<QuestionAttahment>
{
    public void Configure(EntityTypeBuilder<QuestionAttahment> builder)
    {
        builder.HasKey(x => new { x.QuestionId, x.AttachmentId });
        builder.HasOne(x => x.Question).WithMany(x => x.QuestionAttahments).HasForeignKey(x => x.QuestionId);
        builder.HasOne(x => x.Attachment).WithMany(x => x.QuestionAttahments).HasForeignKey(x => x.AttachmentId);
    }
}
