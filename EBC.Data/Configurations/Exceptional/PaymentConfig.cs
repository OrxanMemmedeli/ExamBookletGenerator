using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.Exceptional;

public class PaymentConfig : IEntityTypeConfiguration<PaymentOrDebt>
{
    public void Configure(EntityTypeBuilder<PaymentOrDebt> builder)
    {
        builder.HasOne(x => x.Company)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
