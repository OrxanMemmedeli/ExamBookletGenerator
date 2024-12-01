using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class PaymentOrDebtConfig : BaseEntityConfig<PaymentOrDebt, Guid>
{
    public override void Configure(EntityTypeBuilder<PaymentOrDebt> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CompanyId);

    }
}
