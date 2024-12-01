using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class PaymentSummaryConfig : BaseEntityConfig<PaymentSummary, Guid>
{
    public override void Configure(EntityTypeBuilder<PaymentSummary> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.CompanyId);

    }
}
