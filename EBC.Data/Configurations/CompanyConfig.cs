using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;

namespace EBC.Data.Configurations;

public class CompanyConfig : BaseEntityConfig<Company, Guid>
{
    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.PaymentSummary)
            .WithOne(x => x.Company)
            .HasForeignKey<PaymentSummary>(x => x.CompanyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
