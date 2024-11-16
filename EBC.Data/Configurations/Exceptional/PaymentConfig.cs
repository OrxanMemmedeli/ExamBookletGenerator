using EBC.Data.Entities.ExceptionalEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.Exceptional;

public class PaymentConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasOne(x => x.Company)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
